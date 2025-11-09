# WILLIAM MILLER

Nashville, TN • (615) 630-1806 • williamoutlawmiller@gmail.com • [LinkedIn](https://www.linkedin.com/in/william-miller-b31829214/) • [GitHub](https://github.com/WilliamOutlawMiller)

---

## PROFESSIONAL SUMMARY

Full-stack Software Engineer with two years of experience at AllianceBernstein developing and maintaining applications for Fixed Income municipal bond trading systems. Worked on Python algorithms, C# APIs, and Angular web applications supporting portfolio optimization and trading workflows. Maintained C# APIs as product owner, contributed to Python algorithms, and built CI/CD pipelines using Azure DevOps.

---

## TECHNICAL SKILLS

**Languages:** Python, C#, JavaScript, TypeScript, SQL, HTML/CSS

**Frameworks & Libraries:** .NET Core, .NET 8, .NET Framework 4.7.1, Angular, Flask, Uvicorn, FastAPI, Entity Framework, WCF, SignalR

**DevOps & Infrastructure:** Docker, Kubernetes, Azure DevOps, GitHub Actions, Airflow, Apache, NGINX, YAML, CI/CD Pipelines, TFS

**Databases:** Oracle, SQL Server, MySQL, SQLite

**Specializations:** API Design & Architecture, Multithreading & Concurrency Control, OAuth 2.0 & SSO, Permission-Based Access Control, ETL Pipelines, LLM Integration, Memory Caching, REST APIs

**Platforms:** Linux, Windows

---

## PROFESSIONAL EXPERIENCE

### AllianceBernstein – Fixed Income Technology

**Developer (Contractor via Sans Consulting) | Nashville, TN | November 2023 – November 2025**

Served as product owner for a municipal bond trading optimizer, maintaining the C# API and implementing significant functionality in both the API and Python algorithm. Developed multiple applications from scratch including a securitized assets data pipeline, an AI-powered rate lock data parser, and an automated job monitoring system. Over two years, contributed more code than any other developer to the Optimizer API and Python algorithm. See the Projects section below for detailed descriptions of each project.

#### DevOps & Infrastructure

Assumed DevOps responsibilities for three months following team departures, maintaining and developing build/release infrastructure. Created Azure DevOps CI/CD pipelines using YAML configurations, Dockerfiles, and Python automation scripts for multi-environment deployments. Deployed containerized applications to Kubernetes clusters and Airflow for job orchestration. Configured proxies and reverse proxies, managed Azure service connections, and established Python virtual environments for isolated dependency management.

#### Additional Work

Maintained multiple applications including Python algorithms, .NET Framework 4.7.1 applications with upgrades to .NET Core, and legacy systems using WCF services and TFS. Developed and maintained Python APIs using Uvicorn and Flask, as well as C# APIs. Created and optimized Oracle and SQL Server stored procedures, tables, and indexes for data access.

---

### QuaverEd

**C# Developer | Nashville, TN | 2020 – 2022**

Developed and maintained backend components for a .NET Framework 4.7.1 web application. Implemented data import functionality for school and district data, including bulk CSV processing and relational data mapping. Built queue-based batch processing systems with resume and cancellation capabilities. Integrated multiple third-party data sources and implemented OAuth2 and SSO authentication. Upgraded systems to meet OneRoster v1p1 specification requirements.

---

## PROJECTS

### Muni Optimizer

**Work Project**

Served as product owner for a municipal bond trading optimizer, a Python algorithm that recommended buys and sells for traders to execute. The optimizer included other optimization types such as the Custom SMA model portfolio system. Maintained the C# API and implemented significant functionality in both the API and Python algorithm. Over two years, contributed more code than any other developer to both the Optimizer API and Python algorithm.

The C# API managed concurrency using multithreading, semaphores, read/write locks, monitors, and hashsets to manage CUSIP reservations and prevent race conditions during simultaneous trade offers. Implemented crash recovery functionality using timeout handling on client API calls and database state restoration on API startup. Created new semaphores and locks operating on the static layer of the API.

Assumed ownership over two Angular UIs associated with the optimizer, including a parameter UI that enabled business users to edit optimizer settings such as maximum number of bonds included in the universe. Implemented a permission system that restricted certain settings to manager-level users only. The implementation included backend authentication and authorization detection, and frontend components that disabled editing of restricted settings.

### Securitized Assets Universe Data Pipeline

**Work Project**

Developed from scratch a C# API that consumed data from Kanerai's Securitized Asset API and aggregated asset, deal, tranche, and manager-level aggregate data. The API queried historical data up to 3 years, backfilled the data universe, saved it to databases, and computed custom business metrics including 4-month tail percentages and historical asset weighted average prices.

Created an Excel template using tables to enable data spills on functions. Automated monthly report generation by creating template copies and populating them with processed data. Implemented as a multi-endpoint C# API to enable independent triggering of universe data loading and Excel report generation. Deployed the C# API to a Windows server.

Refactored a Python web application that consumed formatted reports and displayed scatterplots on a webapp UI. Extended functionality from current month uploads only to support historical data uploads. Refactored database and file operations to the server API to resolve datetime coupling issues and prevent deadlocks. Deployed the webapp and batch job to a Windows server, reconfiguring the existing Apache service for the webapp.

### Rate Lock Vendor File Parser

**Work Project**

Developed from scratch an application that processed raw text files containing market Rate Lock data from multiple vendors including Morgan Stanley, Bank of America, and Citi. Previous attempts used manual parsing for each vendor according to a fixed format, but vendor file formats were inconsistent and had evolved over time, making rule-based parsing unreliable.

Implemented an LLM-based solution to parse raw text files into standardized JSON format suitable for database insertion. Containerized the application with a Dockerfile and deployed it to Airflow as a scheduled batch job.

### Automated Job Monitoring & Alerting System

**Work Project**

Developed from scratch a C# console application to replace scheduled email reports containing preformatted HTML tables populated with job data from Control-M's API. The application sends emails to different distribution lists with configurable subject lines and job selections while maintaining consistent HTML table formatting.

Designed a Report model containing Keyname, Days To Run (M-F, T-S, etc.), Schedule (6:00-9:00 AM), Interval (hourly, HalfHourly, SixAndTwelve, Nine), and email-specific settings. Extended the application to support Slack messaging by redefining the Report model with EmailSettings and SlackSettings, enabling reports to send via email, Slack, or both.

Implemented reporting for jobs in Waiting status to identify upstream jobs delaying the network. Defined a ReportType interface to distinguish between report formats, implementing abstract functions like GenerateEmail and GenerateSlackMessage individually for each ReportType while sharing utility functionality including API data access and messaging utilities.

### Songs of Syx

**Mod**

Implemented performance optimizations for the Warhammer Overhaul mod for the Java-based game Songs of Syx, focusing on reducing CPU usage and eliminating frame drops. Developed a memory pooling system to stabilize memory addresses and reduce garbage collection overhead through object reuse. Achieved 40-60% CPU reduction in army AI systems through bitmap deduplication for O(1) membership testing, rate-limited processing with configurable frame budgets, and LIFO queue processing for better responsiveness.

Optimized pathfinding operations by implementing bounded pathfinding queries with configurable limits, preventing unbounded operations that caused frame spikes. Achieved 75% reduction in pathfinding operations through defender reuse prevention and reverse iteration patterns. Enhanced the IUpdater system for staggered updates across multiple frames, implementing gated processing with catch-up caps to prevent frame drops during heavy operations.

Eliminated reflection overhead in hot paths by replacing reflection calls with direct method calls and interface-based dispatch. Implemented queueing systems with priority processing, spatial grouping, and dynamic scaling based on entity count. Reduced memory allocation patterns by optimizing iterator creation in hot paths and implementing pooled collections to minimize GC pressure.

### FantasyLCS WebApp

**Personal Project**

Built a full-stack Razor web application to manage a fantasy league based on Riot Games' LCS esports competition. Developed all frontend, backend, and infrastructure components. The backend API was built with C# and Entity Framework using SQLite and scoped DbContext patterns.

Hosted the application on a Raspberry Pi with NGINX reverse proxy configuration. Implemented SignalR for real-time live score updates during matches. Established a CI/CD pipeline with GitHub Actions to automate deployment. The pipeline stops services, replaces files, and restarts the application on push to the production branch.

### Resume Webapp

**Personal Project**

Built a responsive portfolio website using ASP.NET Core 8.0 MVC with Razor views for server-side rendering. Implemented a custom ResumeService that parses Markdown files using Markdig, extracting structured data including work experience, projects, and technical skills. The service generates HTML with dynamic section IDs for anchor navigation and caches parsed content for performance.

Developed interactive UI components with CSS animations, scroll-triggered effects using Intersection Observer API, and dynamic divider positioning. Created a timeline visualization for work experience with animated markers and hover effects. Implemented custom routing for clean URLs and file download functionality for resume export.

Deployed using Docker containerization with a multi-stage build process. Established a CI/CD pipeline using GitHub Actions that triggers on push to the production branch, SSHs into the deployment server, pulls the latest code, builds the Docker image, and deploys the container. The application runs behind an NGINX reverse proxy.
