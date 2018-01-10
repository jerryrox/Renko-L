#import <Foundation/Foundation.h>

extern "C" {
    
    void _PluginTools_SaveImageToGallery(const char* imagePath) {
        try {
            NSString* newPath = [NSString stringWithUTF8String:imagePath];
            UIImageWriteToSavedPhotosAlbum([UIImage imageWithContentsOfFile:newPath], nil, nil, nil);
        }
        catch(NSException* e) {
            NSLog(@"PluginTools._PluginTools_SaveImageToGallery - Failed: %@", e.reason);
        }
    }
    
    void _PluginTools_CopyToClipboard(const char* text) {
        NSString* newText = [NSString stringWithUTF8String:text];
        UIPasteboard *pasteboard = [UIPasteboard generalPasteboard];
        pasteboard.string = newText;
    }
    
    void _PluginTools_RemoveNetworkCache() {
        [[NSURLCache sharedURLCache] removeAllCachedResponses];
    }
}

