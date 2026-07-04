# Gao Chong - Senior Developer Portfolio (ASP.NET Core MVC)

A modern, highly professional developer portfolio website featuring clean C# architecture, a sleek Tailwind CSS frontend, and interactive web elements flavored with developer humor.

---

## 🚀 Features & Highlights

1. **Retro Hacker Mode:** Press the floating green pixel toggle (`hacker_mode`) at the top right to override standard styles and overlay a green-phosphor retro terminal screen with active matrix-rain filters.
2. **Interactive PowerShell Command Console:** Located on the Resume page, recruiters can query skills, experiences, or request to hire the candidate directly from a simulated console running endpoints on the C# MVC backend.
3. **Witty Preloader & Logs:** Displays humorous status logs (such as verifying CPU pins, checking coolant levels, and bypassing filters) during project loading state.
4. **Strict C# MVC Architecture:** Clean division of concerns with strongly-typed view models, controller routers, and validation data annotations.
5. **Anti-Spam Bot Traps:** Incorporates a hidden HTML Honeypot field in the contact form to catch automated scripts.

---

## 🛠️ Tech Stack

- **Backend:** C# (.NET 8.0 ASP.NET Core MVC)
- **Frontend:** Razor HTML Views, Tailwind CSS CDN (configuration variables extended in layout headers), Vanilla JS / jQuery for client-side routing & dynamic animations.
- **Pattern:** Strict MVC (Models for validation/telemetry, Controllers for endpoints/data repositories, Views for interactive layout representation).

---

## 💻 Running Locally

### 1. Prerequisites
Ensure you have the latest **.NET SDK 8.0** installed on your system. You can verify installation or download it from [dotnet.microsoft.com](https://dotnet.microsoft.com/download):
```bash
dotnet --version
```

### 2. Scaffold Setup
1. Clone or download this project folder into your workspace directory.
2. Open your terminal inside the project directory:
   ```bash
   cd C:/Users/evansleong/.gemini/antigravity/scratch/GaoChongPortfolio
   ```

### 3. Launch Development Server
Restore dependencies and run the application:
```bash
dotnet run
```
Or, start hot-reloading for views and stylesheet edits:
```bash
dotnet watch
```

Once running, navigate your web browser to:
- **HTTPS:** `https://localhost:7100` or `https://localhost:5001`
- **HTTP:** `http://localhost:5000` or `http://localhost:5200`

---

## 📦 Deployment Guide

### Option 1: Azure App Service (Recommended for ASP.NET)
Azure supports native git-based deployments for .NET applications:
1. Create a **Web App** resource in Azure under a free/basic tier (Windows or Linux OS).
2. Install the **Azure App Service** extension in your IDE (VS Code / Visual Studio).
3. Right-click the project folder and click **Deploy to Web App**, or hook up Azure to your Git repository's main branch to trigger auto-builds on push.

### Option 2: Containerization (Docker)
Build and deploy as a Docker image for Kubernetes or AWS ECS:
1. Create a `Dockerfile` in the root:
   ```dockerfile
   FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
   WORKDIR /App

   # Copy everything
   COPY . ./
   # Restore as distinct layers
   RUN dotnet restore
   # Build and publish a release
   RUN dotnet publish -c Release -o out

   # Build runtime image
   FROM mcr.microsoft.com/dotnet/aspnet:8.0
   WORKDIR /App
   COPY --from=build-env /App/out .
   ENTRYPOINT ["dotnet", "GaoChongPortfolio.dll"]
   ```
2. Build and run:
   ```bash
   docker build -t gao-chong-portfolio .
   docker run -d -p 8080:80 --name portfolio gao-chong-portfolio
   ```

### Option 3: Traditional IIS Host
Deploying on standard IIS Server (Windows):
1. Run `dotnet publish -c Release -o ./publish` in terminal.
2. In IIS Manager, create a new site pointing to the `./publish` directory.
3. Verify that the **.NET Hosting Bundle** is installed on the hosting server.
