using Avalonia.Controls;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.IO;
using WinLimit.Services;

namespace WinLimit.ViewModels;

public partial class PopUpWindowViewModel : ViewModelBase
{
    AppBlockerService _appBlockerService;
    private string[] texts = [
        "frankenstein1",
        "frankenstein2"];
    [ObservableProperty]
    private string _textExtract = "";
    [ObservableProperty]
    private string _userInput = "";
    [ObservableProperty]
    private string _ErrorMessage = "";
    private Window _window;
    public PopUpWindowViewModel(AppBlockerService appBlockerService, Window window)
    {
        _appBlockerService=appBlockerService;
        TextExtract = GenerateExtract();
        _window = window;
    }

    [RelayCommand]
    private void StopLoop()
    {
        if (UserInput == TextExtract)
        {
            _appBlockerService.StopLoop();
            _appBlockerService.ChangeOverrideState();
            _window.Close();
        }
        else
        {
            UserInput = "Incorrect input, please try again";
        }
    }

    private string GenerateExtract()
    {
        var Random = new Random();
        int index = Random.Next(texts.Length);
        var uri = new Uri($"avares://WinLimit/Assets/Texts/{texts[index]}.txt"); /*"avares://WinLimit/Assets/offindicator.png"*/
        using Stream stream = AssetLoader.Open(uri);
        using StreamReader reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}