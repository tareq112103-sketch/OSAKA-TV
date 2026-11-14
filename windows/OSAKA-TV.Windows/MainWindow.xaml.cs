using LibVLCSharp.Shared;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace OSAKA_TV.Windows;

public partial class MainWindow : Window
{
    private const string DefaultPlaylistUrl = "https://raw.githubusercontent.com/tareq112103-sketch/OSAKA-TV/main/playlist.m3u";
    private static readonly HttpClient Http = new() { Timeout = TimeSpan.FromSeconds(25) };
    private readonly ObservableCollection<Channel> _visibleChannels = new();
    private readonly HashSet<string> _favourites = new(StringComparer.OrdinalIgnoreCase);
    private readonly LibVLC _libVlc;
    private readonly MediaPlayer _player;
    private Media? _currentMedia;
    private List<Channel> _channels = new();
    private bool _showFavourites;
    private bool _isFullscreen;
    private WindowStyle _oldWindowStyle;
    private WindowState _oldWindowState;

    public MainWindow()
    {
        InitializeComponent();
        Core.Initialize();
        _libVlc = new LibVLC("--network-caching=1500", "--avcodec-hw=any");
        _player = new MediaPlayer(_libVlc);
        VideoView.MediaPlayer = _player;
        ChannelList.ItemsSource = _visibleChannels;
        Loaded += async (_, _) => await LoadRemotePlaylistAsync();
        KeyDown += MainWindow_KeyDown;
        Closed += (_, _) => {
            _player.Stop();
            _currentMedia?.Dispose();
            _player.Dispose();
            _libVlc.Dispose();
        };
    }

    private async Task LoadRemotePlaylistAsync()
    {
        try
        {
            StatusText.Text = "Loading the latest OSAKA TV playlist…";
            var text = await Http.GetStringAsync(DefaultPlaylistUrl);
            SetPlaylist(text, "GitHub playlist loaded");
        }
        catch (Exception ex)
        {
            StatusText.Text = "Could not load the online playlist: " + ex.Message;
            FooterText.Text = "Open a local M3U/M3U8 playlist or check your internet connection.";
        }
    }

    private void SetPlaylist(string text, string status)
    {
        _channels = ParsePlaylist(text);
        _showFavourites = false;
        var groups = _channels.Select(c => c.Group).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        CategoryList.ItemsSource = new[] { "ALL CHANNELS" }.Concat(groups).ToList();
        CategoryList.SelectedIndex = 0;
        FooterText.Text = $"OSAKA TV • {_channels.Count} channels • Playlist source: GitHub / local file";
        StatusText.Text = $"{status}. {_channels.Count} channels found.";
        RefreshChannelList();
    }

    private static List<Channel> ParsePlaylist(string text)
    {
        var result = new List<Channel>();
        string title = "";
        string group = "General";
        string? logo = null;
        foreach (var raw in text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None))
        {
            var line = raw.Trim();
            if (line.StartsWith("#EXTINF", StringComparison.OrdinalIgnoreCase))
            {
                var comma = line.LastIndexOf(',');
                title = comma >= 0 ? line[(comma + 1)..].Trim() : "Unknown channel";
                group = Attribute(line, "group-title") ?? "General";
                logo = Attribute(line, "tvg-logo");
            }
            else if (line.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                     line.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                if (!string.IsNullOrWhiteSpace(title))
                    result.Add(new Channel(title, line, group, logo));
                title = "";
                group = "General";
                logo = null;
            }
        }
        return result;
    }

    private static string? Attribute(string line, string name)
    {
        var match = Regex.Match(line, $@"(?:^|\\s){Regex.Escape(name)}=""([^""]*)""", RegexOptions.IgnoreCase);
        return match.Success ? match.Groups[1].Value : null;
    }

    private void RefreshChannelList()
    {
        var category = CategoryList.SelectedItem as string ?? "ALL CHANNELS";
        var query = SearchBox.Text.Trim();
        IEnumerable<Channel> filtered = _channels;
        if (_showFavourites) filtered = filtered.Where(c => _favourites.Contains(c.Url));
        else if (category != "ALL CHANNELS") filtered = filtered.Where(c => c.Group.Equals(category, StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrWhiteSpace(query))
            filtered = filtered.Where(c => c.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                                           c.Group.Contains(query, StringComparison.OrdinalIgnoreCase));
        _visibleChannels.Clear();
        foreach (var channel in filtered) _visibleChannels.Add(channel);
    }

    private async void Refresh_Click(object sender, RoutedEventArgs e) => await LoadRemotePlaylistAsync();

    private void OpenPlaylist_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog { Title = "Open IPTV playlist", Filter = "M3U playlists|*.m3u;*.m3u8|All files|*.*" };
        if (dialog.ShowDialog() != true) return;
        try { SetPlaylist(File.ReadAllText(dialog.FileName), "Local playlist loaded"); }
        catch (Exception ex) { StatusText.Text = "Playlist error: " + ex.Message; }
    }

    private void CategoryList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        _showFavourites = false;
        RefreshChannelList();
    }

    private void SearchBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        if (IsLoaded) RefreshChannelList();
    }

    private void ChannelList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (ChannelList.SelectedItem is Channel channel) PlayChannel(channel);
    }

    private void Play_Click(object sender, RoutedEventArgs e)
    {
        if (ChannelList.SelectedItem is Channel channel) PlayChannel(channel);
    }

    private void PlayChannel(Channel channel)
    {
        try
        {
            _player.Stop();
            _currentMedia?.Dispose();
            _currentMedia = new Media(_libVlc, channel.Url, FromType.FromLocation);
            _player.Play(_currentMedia);
            NowPlayingText.Text = channel.Title;
            StatusText.Text = $"Playing • {channel.Group}";
            FavoriteButton.Content = _favourites.Contains(channel.Url) ? "★ Favourite" : "☆ Favourite";
        }
        catch (Exception ex) { StatusText.Text = "Playback error: " + ex.Message; }
    }

    private void Stop_Click(object sender, RoutedEventArgs e)
    {
        _player.Stop();
        StatusText.Text = "Playback stopped";
    }

    private void Favorite_Click(object sender, RoutedEventArgs e)
    {
        if (ChannelList.SelectedItem is not Channel channel) return;
        if (!_favourites.Add(channel.Url)) _favourites.Remove(channel.Url);
        FavoriteButton.Content = _favourites.Contains(channel.Url) ? "★ Favourite" : "☆ Favourite";
    }

    private void Favourites_Click(object sender, RoutedEventArgs e)
    {
        _showFavourites = true;
        CategoryList.SelectedIndex = -1;
        RefreshChannelList();
        StatusText.Text = "Showing your favourite channels";
    }

    private void Fullscreen_Click(object sender, RoutedEventArgs e) => ToggleFullscreen();

    private void MainWindow_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.F11) { ToggleFullscreen(); e.Handled = true; }
        else if (e.Key == Key.Escape && _isFullscreen) { ToggleFullscreen(); e.Handled = true; }
        else if (e.Key == Key.Space) { if (_player.IsPlaying) _player.Pause(); else _player.Play(); e.Handled = true; }
    }

    private void ToggleFullscreen()
    {
        if (!_isFullscreen)
        {
            _oldWindowStyle = WindowStyle;
            _oldWindowState = WindowState;
            WindowStyle = WindowStyle.None;
            WindowState = WindowState.Maximized;
            _isFullscreen = true;
        }
        else
        {
            WindowStyle = _oldWindowStyle;
            WindowState = _oldWindowState;
            _isFullscreen = false;
        }
    }

    private sealed record Channel(string Title, string Url, string Group, string? Logo);
}