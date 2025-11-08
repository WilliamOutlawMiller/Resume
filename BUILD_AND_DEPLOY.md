# Build and Deploy Instructions - Windows Side

This guide explains how to build the Docker image on Windows and prepare it for deployment to the Linux server.

## Prerequisites

- Docker Desktop for Windows installed and running
- Git installed (if using version control)
- Access to the Linux server via SSH

## Building the Docker Image

### Option 1: Build Locally

1. Open PowerShell or Command Prompt in the project root directory

2. Build the Docker image:
   ```powershell
   docker build -t williammiller-site:latest .
   ```

3. Verify the image was created:
   ```powershell
   docker images williammiller-site
   ```

### Option 2: Using the Build Script

Run the provided build script:
```powershell
.\build.ps1
```

## Preparing the Image for Transfer

### Option 1: Save Image as Tar File (Recommended)

Save the Docker image to a file that can be transferred to the server:

```powershell
docker save williammiller-site:latest -o williammiller-site.tar
```

This creates a `williammiller-site.tar` file that can be transferred to the Linux server.

**File size note:** The tar file will be large (typically 200-500 MB). Ensure you have enough disk space and a reliable connection for transfer.

### Option 2: Push to Docker Registry

If you have access to a Docker registry (Docker Hub, Azure Container Registry, etc.):

1. Tag the image:
   ```powershell
   docker tag williammiller-site:latest your-registry/williammiller-site:latest
   ```

2. Push to registry:
   ```powershell
   docker push your-registry/williammiller-site:latest
   ```

3. On the Linux server, pull the image:
   ```bash
   docker pull your-registry/williammiller-site:latest
   ```

## Transferring the Image to Linux Server

### Method 1: SCP (Secure Copy)

Using SCP to transfer the tar file:

```powershell
scp williammiller-site.tar bill-criminal@108.254.146.20:/opt/williammiller-site/
```

You'll be prompted for your SSH password.

### Method 2: Using WinSCP or FileZilla

1. Open WinSCP or FileZilla
2. Connect to your server (108.254.146.20)
3. Navigate to `/opt/williammiller-site/`
4. Upload `williammiller-site.tar`

### Method 3: Using PowerShell with SSH

```powershell
# Create directory on server
ssh bill-criminal@108.254.146.20 "mkdir -p /opt/williammiller-site"

# Transfer file
scp williammiller-site.tar bill-criminal@108.254.146.20:/opt/williammiller-site/
```

## Complete Deployment Workflow

### Step 1: Build the Image

```powershell
# Navigate to project directory
cd C:\Users\willi\repos\Resume

# Build Docker image
docker build -t williammiller-site:latest .

# Verify build
docker images williammiller-site
```

### Step 2: Save Image to File

```powershell
docker save williammiller-site:latest -o williammiller-site.tar
```

### Step 3: Transfer to Server

```powershell
# Ensure directory exists on server
ssh bill-criminal@108.254.146.20 "mkdir -p /opt/williammiller-site && chown bill-criminal:bill-criminal /opt/williammiller-site"

# Transfer the tar file
scp williammiller-site.tar bill-criminal@108.254.146.20:/opt/williammiller-site/
```

### Step 4: Deploy on Server

SSH into the server and follow the instructions in `DEPLOYMENT.md`:

```powershell
ssh bill-criminal@108.254.146.20
```

Then on the server:

```bash
cd /opt/williammiller-site
docker load -i williammiller-site.tar
docker compose up -d
# OR use docker run command from DEPLOYMENT.md
```

## Updating the Application

When you make changes to the application:

1. **Build new image:**
   ```powershell
   docker build -t williammiller-site:latest .
   ```

2. **Save to file:**
   ```powershell
   docker save williammiller-site:latest -o williammiller-site.tar
   ```

3. **Transfer to server:**
   ```powershell
   scp williammiller-site.tar bill-criminal@108.254.146.20:/opt/williammiller-site/
   ```

4. **On server, update the container:**
   ```bash
   ssh bill-criminal@108.254.146.20
   cd /opt/williammiller-site
   docker stop williammiller-site
   docker rm williammiller-site
   docker load -i williammiller-site.tar
   docker compose up -d
   ```

## Testing the Image Locally

Before deploying, test the image locally:

```powershell
# Run the container locally
docker run -d -p 8080:80 --name williammiller-site-test williammiller-site:latest

# Test in browser
# Open http://localhost:8080

# Stop and remove test container
docker stop williammiller-site-test
docker rm williammiller-site-test
```

## Troubleshooting

### Docker Build Fails

- Ensure Docker Desktop is running
- Check that all files are present (Dockerfile, .csproj, etc.)
- Review build output for specific errors

### Image File Too Large

- The image includes the .NET runtime and can be large
- Consider using multi-stage builds (already implemented in Dockerfile)
- Check for unnecessary files in the build context

### Transfer Fails

- Check network connection
- Verify SSH access to server
- Ensure sufficient disk space on server
- Check file permissions on server

### Container Won't Start on Server

- Check server logs: `docker logs williammiller-site`
- Verify port 8080 is available
- Check Docker is running: `docker ps`

## Quick Reference Commands

```powershell
# Build
docker build -t williammiller-site:latest .

# Save
docker save williammiller-site:latest -o williammiller-site.tar

# Transfer
scp williammiller-site.tar bill-criminal@108.254.146.20:/opt/williammiller-site/

# Test locally
docker run -d -p 8080:80 --name test williammiller-site:latest

# Clean up
docker rmi williammiller-site:latest
```

