# WinLimit

WinLimit is an open-source digital wellbeing tool built with **Avalonia** and **C#**. It helps users reclaim their time by enforcing scheduled boundaries on distracting applications. This was created for my university course Dissertation project.

## 💡 How it Works
WinLimit allows you to define custom schedules for specific applications. 
- **Enforcement:** When a blocked application is launched during a restricted timeframe, WinLimit uses the standard `.NET Process.Kill()` method to gracefully terminate the session.
- **User Agency:** Users are never "locked out" indefinitely. A visible popup allows for a manual **Override**, giving you control while still providing the friction necessary to break habitual scrolling or gaming.
- **Cross-Device Sync:** Your configurations and blocklists are synced via our secure backend API so your focus remains consistent across all your Windows devices.

## 🛡️ Security & Privacy
- **Open Source:** Licensed under MIT. Our code is transparent and auditable.
- **Signed by SignPath:** This application is digitally signed by the **SignPath Foundation** to ensure the binary you download is exactly what was built from this source code.
- **Data Policy:** We only sync configuration settings (blocklists and schedules). No personal browsing history or activity logs are ever recorded or transmitted.

## 🚀 Installation
[Link to Signed Releases]