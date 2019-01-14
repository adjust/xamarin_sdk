from scripting_utils import *

def build(version, root_dir, ios_submodule_dir, with_test_lib):
    # ------------------------------------------------------------------
    # paths
    srcdir                  = '{0}/sdk'.format(ios_submodule_dir)
    lib_out_dir             = '{0}/ios/AdjustSdk.Xamarin.iOS/Resources'.format(root_dir)
    sdk_static_framework    = '{0}/Frameworks/Static/AdjustSdk.framework'.format(srcdir)
    adjust_api_path         = '{0}/Adjust/Adjust.m'.format(srcdir)

    # ------------------------------------------------------------------
    # Removing old iOS SDK binary
    debug_green('Removing old iOS SDK binary ...')
    remove_file_if_exists('{0}/libAdjust.a'.format(lib_out_dir))

    # ------------------------------------------------------------------
    # Appending SDK prefix to source code
    debug_green('Appending SDK prefix to source code ...')
    replace_text_in_file(adjust_api_path,
        'self.activityHandler = [ADJAdjustFactory activityHandlerWithConfig:adjustConfig', 
        '[adjustConfig setSdkPrefix:@"xamarin{0}"];self.activityHandler = [ADJAdjustFactory activityHandlerWithConfig:adjustConfig'.format(version))
    replace_text_in_file(adjust_api_path,
        'return [[Adjust getInstance] sdkVersion];',
        'return [NSString stringWithFormat: @"xamarin{0}@%@", [[Adjust getInstance] sdkVersion]];'.format(version))

    # ------------------------------------------------------------------
    # Building new iOS SDK binary
    debug_green('Building new iOS SDK binary ...')
    change_dir(srcdir)
    xcode_build('AdjustStatic')

    # ------------------------------------------------------------------
    # Removing SDK prefix from source code
    debug_green('Removing SDK prefix from source code ...')
    replace_text_in_file(adjust_api_path,
        '[adjustConfig setSdkPrefix:@"xamarin{0}"];self.activityHandler = [ADJAdjustFactory activityHandlerWithConfig:adjustConfig'.format(version),
        'self.activityHandler = [ADJAdjustFactory activityHandlerWithConfig:adjustConfig')
    replace_text_in_file(adjust_api_path,
        'return [NSString stringWithFormat: @"xamarin{0}@%@", [[Adjust getInstance] sdkVersion]];'.format(version),
        'return [[Adjust getInstance] sdkVersion];')

    # ------------------------------------------------------------------
    # Copying the generated binary  to lib out dir
    debug_green('Copying the generated binary to {0} ...'.format(lib_out_dir))
    copy_file(sdk_static_framework + '/Versions/A/AdjustSdk', lib_out_dir + '/libAdjust.a')

    if with_test_lib:
        # ------------------------------------------------------------------
        # Test Library paths
        set_log_tag('IOS-TEST-LIB-BUILD')
        debug_green('Building Test Library started ...')
        waiting_animation(duration=4.0, step=0.025)
        test_static_framework   = '{0}/Frameworks/Static/AdjustTestLibrary.framework'.format(srcdir)
        test_lib_out_dir        = '{0}/ios/Test/TestLib/Resources'.format(root_dir)

        # ------------------------------------------------------------------
        # Removing old iOS SDK binary
        debug_green('Removing old iOS SDK binary ...')
        remove_file_if_exists('{0}/libAdjustTest.a'.format(test_lib_out_dir))

        # ------------------------------------------------------------------
        # Building new iOS SDK binary
        debug_green('Building new iOS SDK binary ...')
        change_dir('{0}/AdjustTests/AdjustTestLibrary'.format(srcdir))
        xcode_build('AdjustTestLibraryStatic')
        copy_file(test_static_framework + '/Versions/A/AdjustTestLibrary', test_lib_out_dir + '/libAdjustTest.a')
