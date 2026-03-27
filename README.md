# WinLimit
![Build](https://img.shields.io/github/actions/workflow/status/er-kal/WinLimit/build.yml)
![License](https://img.shields.io/github/license/er-kal/WinLimit)
![Release](https://img.shields.io/github/v/release/er-kal/WinLimit)
![.NET](https://img.shields.io/badge/.NET-9.0-blue)
## Overview
WinLimit is a Windows-native app that helps you control screen time by blocking distracting applications based on custom schedules. The app is built for individuals seeking to manage their screen time, such as students, professionals, or parents monitoring usage.

## Features
- **Application Blocking**: Automatically terminates specified applications when launched during scheduled blocked periods using process monitoring.
- **Weekly Scheduling**: Create and manage time-based rules for each day of the week, with flexible start and end hours.
- **User Authentication**: Secure login and registration with token-based authentication.
- **Cross-Device Synchronization**: Sync blocklists and schedules across Windows devices via a Supabase backend API.
- **Override Mechanism**: Users can manually override blocks through a popup window for temporary access.
- **Local Storage**: Offline functionality with local persistence of settings and schedules.
- **Activity Logging**: Records blocked app attempts for review (configuration and schedules only, no personal activity data).
- **Single Instance Enforcement**: Ensures only one instance of the app runs at a time.

## Installation
### Prerequisites
- Windows 10 or later
- .NET 9.0 Runtime (automatically included in self-contained builds)

### Option 1: Download Pre-built Release
1. Visit the [Releases](https://github.com/yourusername/WinLimit/releases) page.
2. Download the latest `WinLimit.exe`.
3. Run the executable directly - no installation required.

### Option 2: Build from Source
1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/WinLimit.git
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
- **Environment Variables**: Uses `.env` file for API configuration (Supabase URL and keys).

## Architecture
WinLimit follows the MVVM (Model-View-ViewModel) pattern using Avalonia for UI. Key components include:

- **Models**: Data structures for users, schedules, block items, and API responses.
- **Views**: Avalonia XAML-based UI components (MainWindow, LoginPage, SchedulePage, etc.).
- **ViewModels**: Business logic and data binding for each view.
- **Services**:
  - `AppBlockerService`: Core blocking logic with process monitoring and termination.
  - `ScheduleService`: Manages weekly schedules and active state checking.
  - `APIService`: Handles Supabase backend communication for sync and logging.
  - `AuthService`: Manages user authentication and token handling.
  - `LocalStorageService`: Provides local file-based persistence.

The app uses dependency injection for service management and runs a lightweight background loop to enforce active restrictions.

## Technologies Used
- **Language**: C# (.NET 9.0)
- **UI Framework**: Avalonia 11.3.6 (XAML-based)
- **MVVM Toolkit**: CommunityToolkit.Mvvm 8.2.1
- **Backend**: Supabase 1.1.1 (PostgreSQL with real-time features)
- **Dependency Injection**: Microsoft.Extensions.DependencyInjection 10.0.1
- **UI Components**: RangeSlider.Avalonia 2.1.0
- **Security**: System.Security.Cryptography.ProtectedData 10.0.1
- **Configuration**: dotenv.net 4.0.0

## Contributing
1. Fork the repository.
2. Create a feature branch: `git checkout -b feature/your-feature`.
3. Make changes and ensure tests pass.
4. Submit a pull request with a clear description of changes.

Please follow C# coding standards and include appropriate comments. For major changes, open an issue first to discuss.

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Notes
- The app requires administrator privileges for process termination on some systems.
- Blocking is enforced by killing processes gracefully; ensure important work is saved.
- Privacy-focused: Only syncs configuration data (schedules and blocklists) and logs pop up occurences; no browsing history or activity logs are collected.
- Current limitations: Windows-only, no mobile app companion, basic override mechanism without advanced policies.
- TODO: Add unit tests, improve error handling for process killing, implement advanced scheduling options, improve UI