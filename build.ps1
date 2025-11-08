# PowerShell script to build and prepare Docker image for deployment

Write-Host "=== Building William Miller Site Docker Image ===" -ForegroundColor Cyan
Write-Host ""

# Check if Docker is running
Write-Host "Checking Docker status..." -ForegroundColor Yellow
try {
    docker ps | Out-Null
    Write-Host "Docker is running" -ForegroundColor Green
} catch {
    Write-Host "ERROR: Docker is not running. Please start Docker Desktop." -ForegroundColor Red
    exit 1
}

Write-Host ""

# Build the Docker image
Write-Host "Building Docker image..." -ForegroundColor Yellow
docker build -t williammiller-site:latest .

if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: Docker build failed" -ForegroundColor Red
    exit 1
}

Write-Host "Build completed successfully" -ForegroundColor Green
Write-Host ""

# Verify image exists
Write-Host "Verifying image..." -ForegroundColor Yellow
$image = docker images williammiller-site:latest --format "{{.Repository}}:{{.Tag}}"
if ($image -eq "williammiller-site:latest") {
    Write-Host "Image verified: williammiller-site:latest" -ForegroundColor Green
    
    # Show image size
    $size = docker images williammiller-site:latest --format "{{.Size}}"
    Write-Host "Image size: $size" -ForegroundColor Cyan
} else {
    Write-Host "ERROR: Image verification failed" -ForegroundColor Red
    exit 1
}

Write-Host ""

# Ask if user wants to save image to tar file
$saveImage = Read-Host "Save image to tar file for transfer? (y/n)"
if ($saveImage -eq "y" -or $saveImage -eq "Y") {
    Write-Host ""
    Write-Host "Saving image to williammiller-site.tar..." -ForegroundColor Yellow
    docker save williammiller-site:latest -o williammiller-site.tar
    
    if ($LASTEXITCODE -eq 0) {
        $fileSize = (Get-Item williammiller-site.tar).Length / 1MB
        Write-Host "Image saved successfully" -ForegroundColor Green
        Write-Host "File: williammiller-site.tar" -ForegroundColor Cyan
        Write-Host "Size: $([math]::Round($fileSize, 2)) MB" -ForegroundColor Cyan
        Write-Host ""
        Write-Host "Next steps:" -ForegroundColor Yellow
        Write-Host "1. Transfer williammiller-site.tar to the server" -ForegroundColor White
        Write-Host "2. Use: scp williammiller-site.tar bill-criminal@108.254.146.20:/opt/williammiller-site/" -ForegroundColor White
        Write-Host "3. Follow instructions in DEPLOYMENT.md on the server" -ForegroundColor White
    } else {
        Write-Host "ERROR: Failed to save image" -ForegroundColor Red
        exit 1
    }
}

Write-Host ""
Write-Host "=== Build Complete ===" -ForegroundColor Cyan

