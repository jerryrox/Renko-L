#import "RootUtil.h"

@implementation RootUtil

+(BOOL)IsRooted {
    return [RootUtil CheckFiles] || [RootUtil CheckRead] || [RootUtil CheckSandbox];
}

+(BOOL)CheckFiles {
    if ([[NSFileManager defaultManager] fileExistsAtPath:@"/Applications/Cydia.app"] ||
        [[NSFileManager defaultManager] fileExistsAtPath:@"/Library/MobileSubstrate/MobileSubstrate.dylib"] ||
        [[NSFileManager defaultManager] fileExistsAtPath:@"/bin/bash"] ||
        [[NSFileManager defaultManager] fileExistsAtPath:@"/usr/sbin/sshd"] ||
        [[NSFileManager defaultManager] fileExistsAtPath:@"/etc/apt"] ||
        [[NSFileManager defaultManager] fileExistsAtPath:@"/private/var/lib/apt/"] ||
        [[UIApplication sharedApplication] canOpenURL:[NSURL URLWithString:@"cydia://package/com.example.package"]])
            return true;
    return false;
}

+(BOOL)CheckRead {
    FILE *f = NULL ;
    if ((f = fopen("/bin/bash", "r")) ||
        (f = fopen("/Applications/Cydia.app", "r")) ||
        (f = fopen("/Library/MobileSubstrate/MobileSubstrate.dylib", "r")) ||
        (f = fopen("/usr/sbin/sshd", "r")) ||
        (f = fopen("/etc/apt", "r"))) {
        fclose(f);
        return YES;
    }
    fclose(f);
    return NO;
}

+(BOOL)CheckSandbox {
    NSError *error;
    NSString *stringToBeWritten = @"Jailbreak Check.";
    [stringToBeWritten writeToFile:@"/private/jailbreak.txt" atomically:YES
                          encoding:NSUTF8StringEncoding error:&error];
    
    if(error == nil) {
        //Device is jailbroken
        return YES;
    }
    else {
        [[NSFileManager defaultManager] removeItemAtPath:@"/private/jailbreak.txt" error:nil];
    }
    return NO;
}



@end
