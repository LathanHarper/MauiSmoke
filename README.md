# MauiSmoke

MauiSmoke is the smallest useful .NET MAUI iOS TestFlight canary for
CodeCrafty.net. If the app reaches its one screen and displays **BOOTED**, the
SDK, MAUI workload, iOS runtime, signing, App Store Connect upload, TestFlight
delivery, and physical-device launch path all made it across the reef.

## Deliberately tiny

The application targets only `net10.0-ios` and contains:

- stock .NET MAUI;
- a programmatic `Application.CreateWindow` path;
- one programmatic `ContentPage`;
- system fonts and two local SVG build assets; and
- local, non-unique build/runtime/device facts displayed on the screen.

It has no XAML, Prism, Entity Framework, SQLite, custom handlers, telemetry,
networking, preferences, database, or experimental application code. Device
information is displayed locally and is never persisted or transmitted.

## Pinned known-good toolchain

MauiSmoke is the control sample, so it intentionally stays on the same stable
toolchain already proven by the Dapper Dan iOS lane. Preview upgrades belong in
separate experiments after this baseline boots:

| Component | Version |
| --- | --- |
| .NET SDK | `10.0.302` |
| Workload set | `10.0.302.1` |
| .NET MAUI | `10.0.20` |
| .NET for iOS (from the workload set) | `26.5.10315` |
| Xcode | `26.6` |

The exact pins match the proven Dapper Dan lane and keep this control result
reproducible. .NET 10 is an active long-term-support release; the repository
still pins a specific servicing SDK, workload set, and MAUI package instead of
floating to whatever a runner installs next.

## Build lanes

### Public unsigned proof

`.github/workflows/ios-unsigned.yml` runs on pushes, pull requests, and manual
dispatches. It restores the locked graph, builds unsigned simulator and device
application bundles, labels them **RETURN TO SENDER**, and publishes only those
non-installable proof artifacts for seven days. They cannot be submitted to
TestFlight or installed on a physical iPad.

### Internal TestFlight canary

`.github/workflows/testflight.yml` is manual-only, main-branch-only, and gated
by a required `confirm_upload` switch plus a unique numeric build number. It
validates the distribution profile and signed IPA before uploading directly to
TestFlight. The signed IPA is never uploaded as a GitHub artifact. Only a dSYM
archive and symbolication manifest are retained for seven days.

Uploading to TestFlight does not publish an App Store release. Keep the
TestFlight group internal unless you intentionally choose Apple's external beta
review path.

Before enabling the signed lane, create the App Store Connect app and App ID for
`net.codecrafty.mauismoke`, create its App Store distribution provisioning
profile, and protect the GitHub environment named `testflight-canary` so only
trusted `main` builds can access it.

Configure these GitHub environment values without committing their contents:

| Kind | Name |
| --- | --- |
| Secret | `IOS_DISTRIBUTION_CERT_P12_BASE64` |
| Secret | `IOS_DISTRIBUTION_CERT_P12_PASSWORD` |
| Secret | `IOS_APPSTORE_PROFILE_BASE64` |
| Secret | `APPSTORE_API_PRIVATE_KEY` |
| Variable | `IOS_CODESIGN_IDENTITY` |
| Variable | `APPSTORE_ISSUER_ID` |
| Variable | `APPSTORE_API_KEY_ID` |

The workflows are intentionally reusable in shape, but signing material must be
provisioned for this exact bundle ID. Never reuse another app's provisioning
profile.

## Local commands on a Mac

```bash
dotnet workload restore src/MauiSmoke/MauiSmoke.csproj --skip-manifest-update
dotnet restore src/MauiSmoke/MauiSmoke.csproj --locked-mode
dotnet build src/MauiSmoke/MauiSmoke.csproj \
  -f net10.0-ios -c Release -r iossimulator-arm64 \
  --no-restore -p:EnableCodeSigning=false
```

## License

MauiSmoke is public source under the [MIT License](LICENSE). Reuse is welcome;
copies or substantial portions must retain the copyright and permission notice.
Dependency attribution is recorded in
[THIRD-PARTY-NOTICES.md](THIRD-PARTY-NOTICES.md). The internally signed
TestFlight binary is a delivery canary, not a public binary distribution
channel.
