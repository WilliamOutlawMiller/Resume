# William Miller - Professional Developer Website

A modern ASP.NET Core MVC web application showcasing professional developer portfolio.

## Features

- Professional, responsive design
- Headshot display section
- GitHub integration with commit history
- LinkedIn integration with work experience
- Technical skills showcase
- Contact information
- Smooth scrolling navigation

## Technology Stack

- ASP.NET Core 8.0
- MVC (Model-View-Controller) pattern
- Modern CSS with custom properties
- Responsive design

## Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- A web browser

### Running the Application

1. Restore dependencies:
```bash
dotnet restore
```

2. Run the application:
```bash
dotnet run
```

3. Open your browser and navigate to:
   - HTTP: `http://localhost:5000`
   - HTTPS: `https://localhost:5001`

### Development

The application uses the standard ASP.NET Core MVC structure:
- `Controllers/` - MVC controllers
- `Views/` - Razor views
- `wwwroot/` - Static files (CSS, JavaScript, images)
- `Program.cs` - Application entry point

## Deployment

This application is configured for deployment to `williamoutlawmiller.com`. 

### Deployment Options

#### Option 1: IIS (Windows Server)

1. Build the application:
```bash
dotnet publish -c Release -o ./publish
```

2. Copy the `publish` folder contents to your IIS web root directory

3. Ensure the ASP.NET Core Hosting Bundle is installed on the server

4. Configure IIS to use the application pool with .NET CLR Version set to "No Managed Code"

5. Set up HTTPS binding for your domain

#### Option 2: Docker

1. Build the Docker image:
```bash
docker build -t williammiller-site .
```

2. Run the container:
```bash
docker run -d -p 80:80 -p 443:443 --name williammiller-site williammiller-site
```

3. Configure your reverse proxy (NGINX/Apache) to forward requests to the container

#### Option 3: Linux with NGINX

1. Build the application:
```bash
dotnet publish -c Release -o ./publish
```

2. Deploy to your Linux server

3. Configure NGINX as a reverse proxy pointing to the Kestrel server

4. Set up systemd service for the application

5. Configure SSL certificates (Let's Encrypt recommended)

## Customization

### Adding Your Headshot

The site automatically fetches your LinkedIn profile image to use as the headshot. This is handled by the `GetLinkedInImage` controller action which extracts the image URL from your LinkedIn profile's Open Graph meta tags.

If you prefer to use a local image instead, place your headshot at `wwwroot/images/headshot.jpg` and update the image source in `Views/Home/Index.cshtml` to use `~/images/headshot.jpg`. The image should be square and at least 250x250 pixels for best results.

### Updating Social Links

Edit `Views/Home/Index.cshtml` to update GitHub and LinkedIn URLs with your actual profile links.

### LinkedIn Integration

The site uses LinkedIn's official profile badge plugin. The LinkedIn profile URL is set to `https://www.linkedin.com/in/william-miller-b31829214/` in `Views/Home/Index.cshtml`. 

**Note:** LinkedIn does not support direct iframe embedding of work experience sections due to security restrictions. The site uses LinkedIn's official profile badge plugin and provides direct links to the experience section. If you need to display work experience, you can manually extract and display it, or use third-party widgets that integrate with LinkedIn's API.

The headshot image is automatically fetched from your LinkedIn profile's Open Graph image via a server-side controller action (`GetLinkedInImage`). This is cached for 1 hour to improve performance.

### GitHub Integration

The site displays GitHub statistics and activity graphs using the GitHub Readme Stats API. The GitHub username is set to `WilliamOutlawMiller` in `Views/Home/Index.cshtml`. Update this if your GitHub username is different.

These services automatically pull your public GitHub data, so no API keys are required.

## License

Copyright © 2024 William Miller. All rights reserved.

