# Deployment Guide - William Miller Site

This guide provides step-by-step instructions for deploying the William Miller Site to a Linux server using Docker.

## Server Requirements

- Ubuntu 20.04 or later (or similar Linux distribution)
- Docker and Docker Compose installed
- NGINX installed (for reverse proxy and SSL)
- Domain name pointing to server IP (for SSL certificates)
- SSH access to the server

## Server Setup Instructions

### Step 1: Install Docker and Docker Compose

```bash
# Update package index
sudo apt update

# Install prerequisites
sudo apt install -y apt-transport-https ca-certificates curl software-properties-common

# Add Docker's official GPG key
curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo gpg --dearmor -o /usr/share/keyrings/docker-archive-keyring.gpg

# Add Docker repository
echo "deb [arch=$(dpkg --print-architecture) signed-by=/usr/share/keyrings/docker-archive-keyring.gpg] https://download.docker.com/linux/ubuntu $(lsb_release -cs) stable" | sudo tee /etc/apt/sources.list.d/docker.list > /dev/null

# Update package index again
sudo apt update

# Install Docker
sudo apt install -y docker-ce docker-ce-cli containerd.io docker-compose-plugin

# Add current user to docker group (to run docker without sudo)
sudo usermod -aG docker $USER

# Verify Docker installation
docker --version
docker compose version

# Log out and log back in for group changes to take effect
```

### Step 2: Install NGINX

```bash
# Install NGINX
sudo apt install -y nginx

# Start and enable NGINX
sudo systemctl start nginx
sudo systemctl enable nginx

# Verify NGINX is running
sudo systemctl status nginx
```

### Step 3: Create Application Directory

```bash
# Create directory for the application
sudo mkdir -p /opt/williammiller-site
sudo chown $USER:$USER /opt/williammiller-site
cd /opt/williammiller-site
```

### Step 4: Receive Docker Image

The Docker image will be provided in one of the following ways:

**Option A: Docker Image File (Recommended)**
- The image will be transferred as a `.tar` file
- Load it with: `docker load -i williammiller-site.tar`

**Option B: Docker Registry**
- Pull from registry: `docker pull <registry>/williammiller-site:latest`

**Option C: Build from Source**
- If source code is provided, build with: `docker build -t williammiller-site:latest .`

### Step 5: Configure NGINX Reverse Proxy

Create NGINX configuration file:

```bash
sudo nano /etc/nginx/sites-available/williammiller-site
```

Add the following configuration (replace `your-domain.com` with your actual domain):

```nginx
server {
    listen 80;
    server_name your-domain.com www.your-domain.com;

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

Enable the site:

```bash
# Create symbolic link
sudo ln -s /etc/nginx/sites-available/williammiller-site /etc/nginx/sites-enabled/

# Remove default site (optional)
sudo rm /etc/nginx/sites-enabled/default

# Test NGINX configuration
sudo nginx -t

# Reload NGINX
sudo systemctl reload nginx
```

### Step 6: Set Up SSL with Let's Encrypt

```bash
# Install Certbot
sudo apt install -y certbot python3-certbot-nginx

# Obtain SSL certificate (replace with your domain and email)
sudo certbot --nginx -d your-domain.com -d www.your-domain.com --non-interactive --agree-tos --email your-email@example.com

# Certbot will automatically configure NGINX for HTTPS
# Verify SSL certificate renewal is set up
sudo certbot renew --dry-run
```

### Step 7: Deploy the Application

If using docker-compose.yml:

```bash
# Navigate to application directory
cd /opt/williammiller-site

# If docker-compose.yml is provided, use it
docker compose up -d

# Verify container is running
docker ps

# Check logs
docker logs williammiller-site
```

If using Docker directly:

```bash
# Run the container
docker run -d \
  --name williammiller-site \
  --restart unless-stopped \
  -p 8080:80 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e ASPNETCORE_URLS=http://+:80 \
  -e PORT=80 \
  williammiller-site:latest

# Verify container is running
docker ps

# Check logs
docker logs williammiller-site
```

### Step 8: Configure Firewall

```bash
# Allow HTTP and HTTPS traffic
sudo ufw allow 'Nginx Full'
sudo ufw allow OpenSSH
sudo ufw enable

# Verify firewall status
sudo ufw status
```

### Step 9: Verify Deployment

1. Check container status:
   ```bash
   docker ps
   ```

2. Check application logs:
   ```bash
   docker logs williammiller-site
   ```

3. Test local connection:
   ```bash
   curl http://localhost:8080
   ```

4. Test through NGINX:
   ```bash
   curl http://localhost
   ```

5. Visit your domain in a browser to verify the site is accessible

## Updating the Application

When a new version is available:

```bash
# Stop the current container
docker stop williammiller-site
docker rm williammiller-site

# Load new image (if provided as .tar file)
docker load -i williammiller-site.tar

# Or pull new image from registry
docker pull <registry>/williammiller-site:latest

# Start new container
docker compose up -d
# OR
docker run -d \
  --name williammiller-site \
  --restart unless-stopped \
  -p 8080:80 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e ASPNETCORE_URLS=http://+:80 \
  -e PORT=80 \
  williammiller-site:latest

# Check logs
docker logs williammiller-site
```

## Troubleshooting

### Container won't start
```bash
# Check logs
docker logs williammiller-site

# Check if port is already in use
sudo netstat -tlnp | grep 8080

# Remove old container if it exists
docker rm -f williammiller-site
```

### NGINX 502 Bad Gateway
```bash
# Check if container is running
docker ps

# Check container logs
docker logs williammiller-site

# Verify NGINX can reach the container
curl http://127.0.0.1:8080
```

### SSL Certificate Issues
```bash
# Check certificate status
sudo certbot certificates

# Renew certificate manually
sudo certbot renew

# Check NGINX configuration
sudo nginx -t
```

### View Application Logs
```bash
# Follow logs in real-time
docker logs -f williammiller-site

# View last 100 lines
docker logs --tail 100 williammiller-site
```

## Maintenance Commands

```bash
# Restart container
docker restart williammiller-site

# Stop container
docker stop williammiller-site

# Start container
docker start williammiller-site

# Remove container
docker rm williammiller-site

# Remove image
docker rmi williammiller-site:latest

# Clean up unused Docker resources
docker system prune -a
```

## Environment Variables

The application supports the following environment variables:

- `ASPNETCORE_ENVIRONMENT`: Set to `Production` for production deployment
- `ASPNETCORE_URLS`: URL binding (default: `http://+:80`)
- `PORT`: Port number (default: `80`)

## Security Considerations

1. Keep Docker and NGINX updated:
   ```bash
   sudo apt update && sudo apt upgrade -y
   ```

2. Regularly renew SSL certificates (automated with certbot)

3. Monitor application logs for errors

4. Set up firewall rules to restrict access

5. Consider using Docker secrets for sensitive configuration

## Backup and Recovery

To backup the Docker image:

```bash
# Save image to file
docker save williammiller-site:latest | gzip > williammiller-site-backup.tar.gz
```

To restore from backup:

```bash
# Load image from file
gunzip -c williammiller-site-backup.tar.gz | docker load
```

## Support

For issues or questions:
- Check application logs: `docker logs williammiller-site`
- Check NGINX logs: `sudo tail -f /var/log/nginx/error.log`
- Verify container status: `docker ps -a`

