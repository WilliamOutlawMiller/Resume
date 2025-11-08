# WILLIAM MILLER

Nashville, TN • (615) 630-1806 • williamoutlawmiller@gmail.com • [LinkedIn](https://www.linkedin.com/in/william-miller-b31829214/) • [GitHub](https://github.com/WilliamOutlawMiller)

---

## PROFESSIONAL SUMMARY

Full-stack Software Engineer with 2+ years of experience at AllianceBernstein developing and maintaining applications for Fixed Income municipal bond trading systems. Worked on Python algorithms, C# APIs, and Angular web applications supporting portfolio optimization and trading workflows. Experience building APIs, implementing business logic, maintaining legacy systems, and establishing CI/CD pipelines.

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

#### Core Optimizer Platform

- Worked on a municipal bond trading optimizer—a Python-based algorithmic engine that analyzes portfolios and recommends buy/sell trades to maximize returns while adhering to investment constraints

- Maintained the C# .NET API that orchestrated portfolio data retrieval, trade execution workflows, and Custom SMA (Separately Managed Account) model portfolio optimization

- Contributed to the Python recommendation algorithm

- Implemented concurrency controls using multithreading, semaphores, read/write locks, monitors, and hashsets to manage CUSIP reservations and prevent race conditions during simultaneous trade offers

- Developed crash recovery mechanisms with timeout handling and database state restoration on API startup

- Worked on two Angular web applications: a trader-facing UI and a parameter management UI for configuring optimizer settings (bond universe limits, risk thresholds, etc.)

- Implemented role-based permission controls on the parameter UI: added backend user authentication/authorization detection and frontend component logic to disable editing of manager-only settings

#### Securitized Assets Data Pipeline & Reporting System

- Built a C# .NET API to consume data from Kanerai's Securitized Asset API, aggregating asset, deal, tranche, and manager-level data

- Implemented historical data backfill (up to 3 years) with incremental updates, saving structured data to enterprise databases and computing custom business metrics (4-month tail percentages, historical weighted average prices, etc.)

- Created an Excel template with tables to allow for data spills on functions, then created copies of the template and populated them with data for each month, producing monthly reports for business users

- Deployed as a multi-endpoint API on Windows, enabling independent triggering of universe data loading and report generation processes

- Refactored an existing Python web application (from a previous developer) that consumed formatted reports and displayed scatterplots. The application only allowed current month uploads; refactored to accept historical data uploads, moving database and file operations to the server API to prevent deadlocks and client-side crashes

- Deployed both the webapp and batch job to a Windows server, reconfiguring the existing Apache service for the webapp

#### AI-Powered Rate Lock Data Parser

- Took over a legacy text file parser (from a previous developer) that processed Rate Lock data from multiple investment bank vendors (Morgan Stanley, Bank of America, Citi, etc.). The original code manually parsed each vendor according to a set format

- Found that vendor file formats were inconsistent and had evolved over time, making rule-based parsing unreliable

- Implemented an LLM-based solution to parse raw text files into standardized JSON format suitable for database insertion

- Built the application with a Dockerfile and deployed to Airflow to run as a batch job when triggered

#### Automated Job Monitoring & Alerting System

- Created a C# console application for monitoring Control-M job execution across the trading technology network. Queried Control-M API to retrieve job status data and generate HTML-formatted email reports

#### DevOps & Infrastructure

- Worked dedicated on DevOps for three months after DevOps team members left, maintaining and developing build/release infrastructure.

- Created Azure DevOps CI/CD pipelines: authored YAML configurations, Dockerfiles, and Python automation scripts for multi-environment deployments

- Deployed containerized applications to Kubernetes clusters and Airflow for job orchestration

- Configured proxies and reverse proxies, managed service connections, and established Python virtual environments for isolated dependency management

- Maintained legacy TFS-based and WCF service applications, performing upgrades from .NET Framework 4.7.1 to .NET Core/8 where feasible

#### Additional Work

- Developed and maintained Python APIs (Flask, Uvicorn/FastAPI) and C# APIs supporting trading workflows, data transformations, and business analytics

- Worked on .NET Framework 4.7.1 applications including upgrades to .NET Core, deployments, and code maintenance

- Maintained applications with WCF services and using TFS instead of Git

- Created and optimized Oracle and SQL Server stored procedures, tables, and indexes for data access

- Built Python algorithms for portfolio analytics, bond screening, and trade recommendation preprocessing

---

### QuaverEd

**C# Developer | 2020 – 2022**

---

## PROJECTS

### FantasyLCS WebApp

**Personal Project | December 2023 – January 2024 | [GitHub Repository](https://github.com/WilliamOutlawMiller/FantasyLCS)**

- Built a full-stack Razor web application to manage a fantasy league based on Riot Games' LCS esports competition

- Developed all frontend, backend, and infrastructure components; backend API built with C# and Entity Framework using SQLite and scoped DbContext patterns

- Hosted on a Raspberry Pi with NGINX reverse proxy configuration

- Implemented SignalR for real-time live score updates during matches

- Established CI/CD pipeline with GitHub Actions to automate deployment: stops services, replaces files, and restarts the application on push to production branch
