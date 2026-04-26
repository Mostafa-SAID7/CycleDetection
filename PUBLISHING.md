# Publishing to NuGet

## Prerequisites

1. NuGet account at https://www.nuget.org
2. API key from NuGet account settings
3. GitHub repository with secrets configured

## Setup

### 1. Create NuGet API Key

1. Go to https://www.nuget.org/account/apikeys
2. Create new key with "Push new packages and package versions" scope
3. Copy the key

### 2. Add GitHub Secret

1. Go to repository Settings → Secrets and variables → Actions
2. Create new secret: `NUGET_API_KEY`
3. Paste the API key

### 3. Update Package Metadata

Edit `src/CycleDetection/CycleDetection.csproj`:

```xml
<PackageId>CycleDetection</PackageId>
<Version>1.0.0</Version>
<Authors>Your Name</Authors>
<Description>High-performance cycle detection library for 2D grids</Description>
<RepositoryUrl>https://github.com/yourusername/CycleDetection</RepositoryUrl>
```

## Publishing Process

### Automatic (Recommended)

1. Update version in `.csproj`
2. Commit changes: `git commit -m "Bump version to 1.0.0"`
3. Create tag: `git tag v1.0.0`
4. Push: `git push origin main --tags`
5. GitHub Actions automatically publishes to NuGet

### Manual

```bash
# Build package
dotnet pack src/CycleDetection/CycleDetection.csproj -c Release -o ./nupkg

# Publish
dotnet nuget push ./nupkg/CycleDetection.1.0.0.nupkg \
  --api-key YOUR_API_KEY \
  --source https://api.nuget.org/v3/index.json
```

## Versioning Strategy

### Semantic Versioning

- **MAJOR** (1.0.0 → 2.0.0): Breaking changes
  - API changes
  - Removed methods
  - Changed behavior

- **MINOR** (1.0.0 → 1.1.0): New features
  - New implementations
  - New extension methods
  - Backward compatible

- **PATCH** (1.0.0 → 1.0.1): Bug fixes
  - Performance improvements
  - Bug fixes
  - Documentation updates

### Version Numbering

```
1.0.0-alpha.1    (Pre-release)
1.0.0-beta.1     (Pre-release)
1.0.0-rc.1       (Release candidate)
1.0.0            (Stable release)
```

## Release Checklist

- [ ] Update version in `.csproj`
- [ ] Update CHANGELOG.md
- [ ] Run full test suite: `dotnet test`
- [ ] Run benchmarks: `dotnet run -c Release` (benchmarks)
- [ ] Update README.md if needed
- [ ] Commit changes
- [ ] Create git tag
- [ ] Push to main
- [ ] Verify GitHub Actions workflow
- [ ] Check NuGet.org for package

## Troubleshooting

### Package Already Exists

If you get "The package with version X already exists", either:
1. Increment version number
2. Delete package from NuGet.org (requires ownership)

### API Key Issues

- Verify API key is correct
- Check key has "Push" permission
- Ensure key hasn't expired

### Build Failures

- Run `dotnet clean`
- Run `dotnet restore`
- Check .NET SDK version: `dotnet --version`

## Continuous Integration

The GitHub Actions workflow automatically:

1. Restores dependencies
2. Builds in Release mode
3. Runs all tests
4. Creates NuGet package
5. Publishes on tag push

Workflow file: `.github/workflows/build-and-publish.yml`

## Package Maintenance

### Update Existing Package

1. Make changes
2. Increment version (PATCH or MINOR)
3. Commit and tag
4. Push to trigger workflow

### Deprecate Package

1. Mark as deprecated on NuGet.org
2. Update package description
3. Release new version with deprecation notice

### Delisting Package

1. Go to NuGet.org package page
2. Click "Manage"
3. Unlist package (not deleted, just hidden)

## Documentation

- README.md: Usage guide
- ARCHITECTURE.md: Design details
- PERFORMANCE.md: Optimization info
- XML comments: API documentation

All documentation is included in the NuGet package.
