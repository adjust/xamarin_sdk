using ObjCRuntime;

[assembly: LinkWith ("libAdjust.a", LinkTarget.Simulator | LinkTarget.ArmV7 | LinkTarget.ArmV7s | LinkTarget.Simulator64 | LinkTarget.Arm64, SmartLink = true, ForceLoad = false, Frameworks = "AdSupport iAd")]
