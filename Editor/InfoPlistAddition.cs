using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using UnityEditor.iOS.Xcode;

public class InfoPlistAddition {

	[PostProcessBuild]
	public static void ChangeXcodePlist(BuildTarget buildTarget, string pathToBuiltProject) {
		if(buildTarget == BuildTarget.iOS) {
			string plistPath = pathToBuiltProject + "/Info.plist";
			PlistDocument plist = new PlistDocument();
			plist.ReadFromString( File.ReadAllText(plistPath) );

			PlistElementDict rootDict = plist.root;

			//Arbitrary loads
			PlistElementDict appTransportSec = rootDict.CreateDict("NSAppTransportSecurity");
			appTransportSec.SetBoolean("NSAllowsArbitraryLoads", true);

			//Camera
			rootDict.SetString("NSCameraUsageDescription", "Used for taking photos and videos.");

			//Microphone
			rootDict.SetString("NSMicrophoneUsageDescription", "Used for taking videos.");

			//Photo Library
			rootDict.SetString("NSPhotoLibraryUsageDescription", "Used for saving photos and videos.");
			rootDict.SetString("NSPhotoLibraryAddUsageDescription", "Used for adding photos / videos to the library.");

			File.WriteAllText(plistPath, plist.WriteToString());
		}
	}
}
