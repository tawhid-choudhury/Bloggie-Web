# Bloggie

Bloggie is a modern blogging platform built with ASP.NET Core Razor Pages (.NET 9). It supports user registration, authentication, blog post management, image uploads, likes, comments, and role-based access control.

## Features

- **User Registration & Login:** Secure registration and login using ASP.NET Core Identity.
- **Role Management:** Assign roles (e.g., User, Admin) to control access to features.
- **Blog Post Management:** Create, edit, delete, and view blog posts.
- **Image Uploads:** Upload and manage images using Cloudinary integration.
- **Likes & Comments:** Users can like and comment on blog posts.
- **Validation:** Server-side and client-side validation for forms.
- **Responsive UI:** Clean, responsive design using Bootstrap.
- **Error Handling:** Friendly error pages and validation messages.
- **Secure:** Uses HTTPS, strong password policies, and secure authentication.

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Express or higher)
- [Cloudinary Account](https://cloudinary.com/) (for image uploads)
- (Optional) [Visual Studio 2022](https://visualstudio.microsoft.com/vs/)

## Getting Started

### 1. Clone the Repository

```
git clone https://github.com/yourusername/bloggie.git cd bloggie
```

### 2. Configure User Secrets

Set up your connection strings and Cloudinary credentials using [User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets):

#### Initialize user secrets for the project

```
dotnet user-secrets init
```

#### Set database connection strings

```
dotnet user-secrets set "ConnectionStrings:BloggieDbConnectionString" "Server=YOUR_SERVER;Database=BloggieDb;Trusted_Connection=True;TrustServerCertificate=Yes"
dotnet user-secrets set "ConnectionStrings:BloggieAuthDbConnectionString" "Server=YOUR_SERVER;Database=BloggieAuthDb;Trusted_Connection=True;TrustServerCertificate=Yes"
```

#### Set Cloudinary credentials

```
dotnet user-secrets set "Cloudinary:CloudName" "your_cloud_name"
dotnet user-secrets set "Cloudinary:ApiKey" "your_api_key"
dotnet user-secrets set "Cloudinary:ApiSecret" "your_api_secret"
```

Or, edit the `secrets.json` file directly (not recommended for production).

### 3. Restore Packages

```
dotnet restore
```

### 4. Apply Migrations

Update the databases with Entity Framework Core migrations:

```
dotnet ef database update --context BloggieDbContext dotnet ef database update --context AuthDbContext
```

### 5. Run the Application

```
dotnet run
```

## Usage

#### As a Super Admin

- Log in as superadmin to access blog features, update, add and delete blogs users tags.

**Super Admin Credentials**

```
Username (Email): superadmin@bloggie.com
Password: Superadmin@123
```

- Create, edit, and delete blog posts (admin features may require role assignment).
- Upload images to Cloudinary.
- Like and comment on posts.

## Deployment

- Update connection strings for your production SQL Server.
- Set environment variables or use a secure secrets manager for sensitive data.
- Publish using `dotnet publish` and deploy to your preferred host (Azure, IIS, etc.).

## Troubleshooting

- Ensure SQL Server is running and accessible.
- Check Cloudinary credentials for image upload issues.
- Review console/log output for detailed error messages.
