# William Miller - Professional Developer Website

ASP.NET Core MVC web application for showcasing professional developer portfolio.

## Technology Stack

- ASP.NET Core 8.0
- MVC (Model-View-Controller) pattern
- Markdig for markdown processing
- Docker for containerization
- NGINX as reverse proxy
- GitHub Actions for CI/CD

## Installation

### Prerequisites

- .NET 8.0 SDK or later
- Docker and Docker Compose (for deployment)
- Git

### Local Development

```bash
dotnet restore
dotnet run
```

Application runs at:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`

## Server Configuration

### Initial Server Setup

**Server Details:**
- Domain: `williamoutlawmiller.com`
- Server IP: `108.254.146.20`
- Server User: `bill-criminal`
- Application Directory: `/opt/williammiller-site`

**Required Software Installation:**

```bash
# Install Docker and Docker Compose
sudo apt update
sudo apt install -y apt-transport-https ca-certificates curl software-properties-common
curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo gpg --dearmor -o /usr/share/keyrings/docker-archive-keyring.gpg
echo "deb [arch=$(dpkg --print-architecture) signed-by=/usr/share/keyrings/docker-archive-keyring.gpg] https://download.docker.com/linux/ubuntu $(lsb_release -cs) stable" | sudo tee /etc/apt/sources.list.d/docker.list > /dev/null
sudo apt update
sudo apt install -y docker-ce docker-ce-cli containerd.io docker-compose-plugin

# Add user to docker group
sudo usermod -aG docker bill-criminal
newgrp docker

# Install NGINX
sudo apt install -y nginx
sudo systemctl start nginx
sudo systemctl enable nginx

# Create application directory
sudo mkdir -p /opt/williammiller-site
sudo chown bill-criminal:bill-criminal /opt/williammiller-site
```

### NGINX Configuration

Create `/etc/nginx/sites-available/williammiller-site`:

```nginx
server {
    listen 80;
    server_name williamoutlawmiller.com www.williamoutlawmiller.com;

    location / {
        proxy_pass http://127.0.0.1:8080;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
        proxy_cache_bypass $http_upgrade;
    }
}
```

Enable site:

```bash
sudo ln -s /etc/nginx/sites-available/williammiller-site /etc/nginx/sites-enabled/
sudo rm /etc/nginx/sites-enabled/default
sudo nginx -t
sudo systemctl reload nginx
```

### SSL Certificate Setup

```bash
sudo apt install -y certbot python3-certbot-nginx
sudo certbot --nginx -d williamoutlawmiller.com -d www.williamoutlawmiller.com --non-interactive --agree-tos --email williamoutlawmiller@gmail.com
sudo certbot renew --dry-run
```

### Firewall Configuration

```bash
sudo ufw allow 'Nginx Full'
sudo ufw allow OpenSSH
sudo ufw enable
sudo ufw status
```

### DNS Configuration

**Option 1: Direct DNS (Squarespace) - Requires ports in URL**

Add DNS records in Squarespace:
- **A Record:** `@` → `108.254.146.20`
- **A Record:** `www` → `108.254.146.20`

**Note:** With direct DNS, users must access the site with port numbers: `https://williamoutlawmiller.com:8443`

**Option 2: Cloudflare Reverse Proxy - Standard ports without port numbers**

Cloudflare acts as a reverse proxy, accepting traffic on standard ports (80/443) and forwarding to your server on ports 8080/8443. This allows users to access your site without port numbers.

**Option 3: Self-Hosted Solutions (No Third-Party Service)**

If you want to avoid third-party services, you have these options:

**A. Contact Your ISP:**
- Call your ISP and request they unblock ports 80/443
- Residential ISPs typically block these ports to prevent web hosting
- You may need to upgrade to a business internet plan
- Business plans usually don't block ports 80/443

**B. Use a VPS/Cloud Provider:**
- Move your server to a cloud provider (DigitalOcean, Linode, AWS, etc.)
- Cloud providers don't block ports 80/443
- You can use standard ports directly
- Cost: ~$5-10/month for a basic VPS

**C. Set Up Your Own Reverse Proxy on a VPS:**
- Rent a small VPS (DigitalOcean, Linode, etc.) for ~$5/month
- Install NGINX on the VPS
- Configure NGINX to proxy to your home server on ports 8080/8443
- Point DNS to the VPS IP
- Users access the VPS on standard ports, VPS forwards to your home server
- This is essentially self-hosting your own Cloudflare

**D. Accept Port Numbers in URL:**
- Keep current setup with ports 8080/8443
- Users access: `https://williamoutlawmiller.com:8443`
- This is the simplest solution but requires port numbers in URLs

**Recommendation:**
For a public website, Option B (VPS/Cloud Provider) is the most practical solution. It's inexpensive, reliable, and doesn't require port numbers or third-party services.

**Setup Steps:**

1. **Sign up for Cloudflare** (free): https://www.cloudflare.com/

2. **Add your domain to Cloudflare:**
   - Go to Cloudflare Dashboard → Add a Site
   - Enter `williamoutlawmiller.com`
   - Choose the Free plan

3. **Update DNS records in Cloudflare:**
   - Go to DNS → Records
   - Add A record: `@` → `108.254.146.20` (Proxy enabled - orange cloud)
   - Add A record: `www` → `108.254.146.20` (Proxy enabled - orange cloud)
   - **Important:** Make sure the proxy is enabled (orange cloud icon)

4. **Update nameservers:**
   - Cloudflare will provide nameservers (e.g., `ns1.cloudflare.com`, `ns2.cloudflare.com`)
   - Update your domain registrar (Squarespace) to use Cloudflare's nameservers
   - This may take 24-48 hours to propagate

5. **Configure SSL/TLS:**
   - Go to SSL/TLS → Overview
   - Set encryption mode to "Full" or "Full (strict)"
   - This ensures Cloudflare connects to your server via HTTPS on port 8443

6. **Configure Cloudflare to use your custom ports:**
   - Go to SSL/TLS → Origin Server
   - Create an Origin Certificate (optional, but recommended)
   - Or configure Cloudflare to connect to your server on port 8443
   - Go to Network → Port Configuration
   - Set HTTPS port to `8443` (if available in your plan)

7. **Alternative: Use Cloudflare Tunnel (Cloudflared)**
   - Install cloudflared on your server
   - Create a tunnel that connects Cloudflare to your server on port 8080/8443
   - This bypasses the need for port forwarding entirely

**Benefits of Cloudflare:**
- Users access site on standard ports (no `:8443` needed)
- Free SSL certificates
- DDoS protection
- CDN and caching
- Hides your server IP
- Works even if ISP blocks ports 80/443

**Verify DNS propagation:**
```bash
nslookup williamoutlawmiller.com
nslookup www.williamoutlawmiller.com
```

## GitHub Actions CI/CD Setup

### GitHub Secrets Configuration

In repository Settings → Secrets and variables → Actions, add:

1. **SERVER_HOST**: `108.254.146.20`
2. **SERVER_USER**: `bill-criminal`
3. **SSH_PRIVATE_KEY**: Contents of SSH private key (full key including BEGIN/END lines)
4. **SSH_PORT**: `22`
5. **GITHUB_PAT**: (Optional) GitHub Personal Access Token for private repositories

### SSH Key Setup

Generate SSH key for GitHub Actions:

```bash
ssh-keygen -t ed25519 -C "github-actions" -f ~/.ssh/github_actions_deploy
```

Copy public key to server:

```bash
ssh-copy-id -i ~/.ssh/github_actions_deploy.pub bill-criminal@108.254.146.20
```

Add private key content to GitHub Secret `SSH_PRIVATE_KEY`.

### Workflow Behavior

The GitHub Actions workflow automatically:
1. Triggers on push to `prod` branch
2. SSHs into server
3. Pulls latest code from GitHub
4. Builds Docker image on server
5. Deploys new container using docker-compose
6. Verifies deployment

## Deployment

### Automated Deployment (GitHub Actions)

Push to `prod` branch triggers automatic deployment.

### Manual Deployment

```bash
cd /opt/williammiller-site
docker compose up -d
docker ps
docker logs williammiller-site
```

### Updating Application

```bash
cd /opt/williammiller-site
docker stop williammiller-site
docker rm williammiller-site
docker compose up -d
docker logs williammiller-site
```

## Troubleshooting

### Container Won't Start

```bash
docker logs williammiller-site
sudo netstat -tlnp | grep 8080
docker ps -a
```

### NGINX 502 Bad Gateway

```bash
docker ps
curl http://127.0.0.1:8080
sudo nginx -t
sudo tail -f /var/log/nginx/error.log
```

### Connection Refused

```bash
sudo ufw status
sudo systemctl status nginx
sudo netstat -tlnp | grep :80
docker ps
```

### SSL Certificate Issues

```bash
sudo certbot certificates
sudo certbot renew
sudo nginx -t
```

## Application Structure

- `Controllers/` - MVC controllers
- `Services/` - Business logic (ResumeService)
- `Views/` - Razor views
- `wwwroot/` - Static files (CSS, JavaScript, images)
- `resume.md` - Resume content in markdown format
- `Program.cs` - Application entry point
- `Dockerfile` - Docker image configuration
- `docker-compose.yml` - Docker Compose configuration

## Customization

### Updating Resume Content

Edit `resume.md` file. The application automatically parses and displays the content.

### Updating Social Links

Edit `Views/Home/Index.cshtml` to update GitHub and LinkedIn URLs.

### Styling

CSS files are in `wwwroot/css/`:
- `site.css` - Main site styles
- `resume.css` - Resume page specific styles

## Maintenance

### Update Docker

```bash
sudo apt update && sudo apt upgrade -y
```

### Clean Up Docker Resources

```bash
docker system prune -a
```

### View Logs

```bash
docker logs -f williammiller-site
sudo tail -f /var/log/nginx/error.log
```

## License

Copyright © 2024 William Miller. All rights reserved.
