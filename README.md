# WinLimit
![Build](https://img.shields.io/github/actions/workflow/status/er-kal/WinLimit/build.yml)
![License](https://img.shields.io/github/license/er-kal/WinLimit)
![Release](https://img.shields.io/github/v/release/er-kal/WinLimit)
![.NET](https://img.shields.io/badge/.NET-9.0-blue)
## Overview
WinLimit is a Windows-native app that helps you control screen time by blocking distracting applications based on custom schedules. The app is built for individuals seeking to manage their screen time, such as students, professionals, or parents monitoring usage.

## What it does
- **Block apps**: Select blocks you want to app, and it'll close them as they launch
- **Scheduling**: Select when you want apps to be blocked based on a weekly schedule
- **Accounts**: Create an account so you can keep your settings across multiple devices
- **Friction-Layer**: In the case you want to want to disable the block, theres a "mini-game"esque pop-up that can grant temporary access
- **Offline Functionality**: Works fully offline
- **Activity Logging**: Records blocked app attempts for review (configuration and schedules only, no personal activity data).

## Installation
### Prerequisites
- Windows 10 or later
- .NET 9.0 Runtime (automatically included in self-contained builds)

### Option 1: Download Pre-built Release
1. Visit the [Releases](https://github.com/er-kal/WinLimit/releases) page.
2. Download the latest `WinLimit.exe`.
3. Run the executable directly - no installation required.

### Option 2: Build from Source
1. Clone the repository:
   ```bash
   git clone https://github.com/er-kal/WinLimit.git
   cd WinLimit
   ```
2. Restore dependencies:
   ```bash
   dotnet restore
   ```
3. Build and run:
   ```bash
   dotnet build
   dotnet run
   ```

## Usage
1. **Launch the App**: Run `WinLimit.exe` or `dotnet run` from the project directory.
2. **Login/Register**: Create an account or log in to sync settings across devices.
3. **Set Schedules**: Navigate to the Schedule page to define blocked time slots for each weekday.
4. **Manage Blocklist**: Go to the Block List page to add applications by executable name.
5. **Monitor Activity**: The app runs in the background, automatically blocking apps during scheduled times.
6. **Override Blocks**: When an app is blocked, a popup appears allowing temporary override.

Example Workflow:
- Block social media apps (e.g., `chrome.exe` for web browsers) from 9 PM to 6 AM daily.
- Add gaming executables during work hours on weekdays.
- Review blocked attempts in the home dashboard.

## Configuration
- **Schedules**: Configured via the Schedule page UI, stored locally and synced to cloud.
- **Blocklist**: Managed through the Block List page, supports custom app names and descriptions.
- **Authentication**: Tokens stored securely using Windows Data Protection API.
- **Self-Hosting**: Change the API URL if you'd prefer to self-host.

## Technologies Used
- **Language**: C# (.NET 9.0)
- **UI Framework**: Avalonia 11.3.6 (XAML-based)
- **MVVM Toolkit**: CommunityToolkit.Mvvm 8.2.1
- **Backend**: Supabase 1.1.1 (PostgreSQL with real-time features)
- **Dependency Injection**: Microsoft.Extensions.DependencyInjection 10.0.1
- **UI Components**: RangeSlider.Avalonia 2.1.0
- **Security**: System.Security.Cryptography.ProtectedData 10.0.1
- **Configuration**: dotenv.net 4.0.0

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Notes
- The app requires administrator privileges for process termination on some systems.
- Blocking is enforced by killing processes gracefully; ensure important work is saved.
- Privacy-focused: Only syncs configuration data (schedules and blocklists) and logs pop up occurences; no browsing history or activity logs are collected.
- Current limitations: Windows-only, no mobile app companion, basic override mechanism without advanced policies.
- TODO: Add unit tests, improve error handling for process killing, implement advanced scheduling options, improve UI