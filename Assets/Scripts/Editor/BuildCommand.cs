using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;

public static class BuildCommand {
    static void PerformBuild_OSx() {
        var options = new BuildPlayerOptions {
            locationPathName = "Build/os_x/game.app",
            scenes = new[] { "Assets/Scenes/Start.unity", "Assets/Scenes/MainMenu.unity", "Assets/Scenes/OnePlayer.unity" },
            target = BuildTarget.StandaloneOSX
        };

        var report = BuildPipeline.BuildPlayer(options);
        var summary = report.summary;

        if (summary.result == BuildResult.Succeeded) {
            Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
            Debug.Log("Path: " + summary.outputPath);
        }

        if (summary.result == BuildResult.Failed) {
            Debug.Log("Build failed");
            EditorApplication.Exit(1);
        }
    }
    static void PerformBuild_Windows() {
        var options = new BuildPlayerOptions {
            locationPathName = "Build/windows.exe",
            scenes = new[] { "Assets/Scenes/Start.unity", "Assets/Scenes/MainMenu.unity", "Assets/Scenes/OnePlayer.unity" },
            target = BuildTarget.StandaloneWindows64
        };

        var report = BuildPipeline.BuildPlayer(options);
        var summary = report.summary;

        if (summary.result == BuildResult.Succeeded) {
            Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
            Debug.Log("Path: " + summary.outputPath);
        }

        if (summary.result == BuildResult.Failed) {
            Debug.Log("Build failed");
            EditorApplication.Exit(1);
        }
    }
    static void PerformBuild_Linux() {
        var options = new BuildPlayerOptions {
            locationPathName = "Build/linux",
            scenes = new[] { "Assets/Scenes/Start.unity", "Assets/Scenes/MainMenu.unity", "Assets/Scenes/OnePlayer.unity" },
            target = BuildTarget.StandaloneLinux64
        };

        var report = BuildPipeline.BuildPlayer(options);
        var summary = report.summary;

        if (summary.result == BuildResult.Succeeded) {
            Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
            Debug.Log("Path: " + summary.outputPath);
        }

        if (summary.result == BuildResult.Failed) {
            Debug.Log("Build failed");
            EditorApplication.Exit(1);
        }
    }        
}