# William Miller - Professional Developer Website

ASP.NET Core MVC web application for showcasing professional developer portfolio.

## Technology Stack

- ASP.NET Core 8.0
- MVC (Model-View-Controller) pattern
- Markdig for markdown processing
- Docker for containerization
- [Render](https://render.com) for managed hosting (Docker web service; see [`render.yaml`](./render.yaml) and [`docs/devops-render.md`](./docs/devops-render.md))
- NGINX as reverse proxy (optional self-hosted path; see example [`nginx-server.conf`](./nginx-server.conf))
- GitHub Actions workflow for **optional** self-hosted SSH deploy only (disabled for automatic runs; Render does not use it)

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

## Deploy on Render

Production hosting is intended to run on **Render** as a Docker web service.

1. Push this repository (including [`render.yaml`](./render.yaml)) to GitHub.
2. In [Render](https://dashboard.render.com), choose **New → Blueprint**.
3. Connect the repository and select the branch that should deploy (the blueprint defaults to `prod`; edit `render.yaml` if your deploy branch differs).
4. Confirm the service name, region, and instance type, then create the blueprint.
5. After the first successful deploy, open the web service → **Settings → Custom Domains** and attach your domain; follow Render’s DNS instructions for TLS.

Operational detail (environment variables, health checks, domains, logs, rollbacks) is documented in **[`docs/devops-render.md`](./docs/devops-render.md)** in the same structure Render uses for web services and blueprints.

**Optional — Deploy to Render button:** After the repo is public, you can add a “Deploy to Render” button that points at `https://render.com/deploy` with your repo URL as documented in [Deploy to Render](https://render.com/docs/deploy-to-render).

## Self-hosted server (optional)

If you deploy to your own VPS instead of Render, install Docker, Docker Compose, and optionally NGINX. Replace every placeholder below with your own values — **do not commit real IPs, SSH accounts, or paths** to a public repository.

**Placeholders:** `YOUR_SERVER_IP`, `YOUR_SSH_USER`, `YOUR_DOMAIN`, `you@example.com`, `/opt/your-app`, host port mapped from the app (e.g. `8081` as in [`docker-compose.yml`](./docker-compose.yml)).

**NGINX:** Copy and edit [`nginx-server.conf`](./nginx-server.conf) (defaults use `example.com`); adjust `server_name`, certificate paths, and `proxy_pass` to match your app port.

**SSL (Certbot example):**

```bash
sudo certbot --nginx -d YOUR_DOMAIN -d www.YOUR_DOMAIN --non-interactive --agree-tos --email you@example.com
```

## GitHub Actions (self-hosted only)

**Render-only:** You do **not** need GitHub Actions **secrets**, **variables**, or **environments** for deployment. Connect the repo in Render; pushes to your deploy branch trigger Render builds. Add a custom domain under the web service in the Render dashboard and point DNS there—no `DOMAIN` or `example.com` environment variable is required for this app (it does not read those for routing).

**Self-hosted (reference):** The workflow [`.github/workflows/deploy.yml`](./github/workflows/deploy.yml) is a generic template (`example-app` names, default branch `main`). It runs **only when you start it manually** (Actions → **Example — self-hosted SSH deploy (reference)** → Run workflow). Edit the `env` block in that file and align [`docker-compose.yml`](./docker-compose.yml) service/image/container names if you use it. Set repository variable `DEPLOY_GIT_BRANCH` if your deploy branch is not `main` (for example `prod`).

Use **real** values in GitHub (your server IP, domain, email)—not the literal string `example.com` unless that is your domain. The `example.com` / `you@example.com` strings in this README are documentation placeholders for nginx and Certbot; they are not copied into Render.

### Secrets (Settings → Secrets and variables → Actions → Secrets)

| Name | Description |
| --- | --- |
| `SERVER_HOST` | Server hostname or IP |
| `SERVER_USER` | SSH user for deployment |
| `SSH_PRIVATE_KEY` | Full PEM for the deploy key |
| `SSH_PORT` | SSH port (often `22`) |
| `GITHUB_PAT` | Optional PAT if the repo must be cloned with HTTPS auth |

### Variables (Settings → Secrets and variables → Actions → Variables)

| Name | Example | Used for |
| --- | --- | --- |
| `DEPLOY_APP_DIR` | `/opt/your-app` | Remote directory for the app |
| `DEPLOY_CONTAINER_PORT` | `8081` | Host port NGINX proxies to |
| `DEPLOY_DOMAIN` | `example.com` | Domain for NGINX / Certbot |
| `CERT_EMAIL` | `you@example.com` | Let's Encrypt registration |
| `DEPLOY_GIT_BRANCH` | `main` | Git branch to deploy (omit to default to `main`; use `prod` if that is your deploy branch) |

### SSH key setup

```bash
ssh-keygen -t ed25519 -C "github-actions" -f ~/.ssh/github_actions_deploy
ssh-copy-id -i ~/.ssh/github_actions_deploy.pub YOUR_SSH_USER@YOUR_SERVER_IP
```

Add the **private** key contents to `SSH_PRIVATE_KEY`.

### Workflow behavior

When you run the workflow manually, it SSHs to the server, syncs the repository, runs `docker compose`, and runs the NGINX deploy script when present.

## Deployment

### Render

Push to the branch connected to the Render service (see `render.yaml`). Use the Render dashboard for manual deploys and logs.

### Self-hosted via GitHub Actions

Configure the secrets and variables above, then run **Example — self-hosted SSH deploy (reference)** from the Actions tab.

### Self-hosted manual Docker

```bash
cd /opt/your-app   # your DEPLOY_APP_DIR
docker compose up -d
docker ps
docker logs <container-name>
```

### Updating the application (self-hosted)

```bash
cd /opt/your-app
docker compose down
docker compose up -d --build
docker logs <container-name>
```

## Troubleshooting

### Container Won't Start

```bash
docker logs <container-name>
sudo ss -tlnp | grep -E '8080|8081'
docker ps -a
```

### NGINX 502 Bad Gateway

```bash
docker ps
curl http://127.0.0.1:8081
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
- `render.yaml` - Render Blueprint (IaC for the Docker web service)
- `docs/devops-render.md` - Runbook for hosting on Render

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
docker logs -f <container-name>
sudo tail -f /var/log/nginx/error.log
```

## License

Copyright © 2024 William Miller. All rights reserved.
