import os, subprocess
from scripting_utils import *

def build(sdk_prefix, root_dir, ios_submodule_dir, with_test_lib):
    # ------------------------------------------------------------------
    # paths
    srcdir                  = '{0}/sdk'.format(ios_submodule_dir)
    lib_out_dir             = '{0}/ios/AdjustSdk.Xamarin.iOS/Resources'.format(root_dir)
    sdk_static_framework    = '{0}/Frameworks/Static/AdjustSdk.framework'.format(srcdir)
    project_dir             = '{0}/ext/ios/sdk/Adjust'.format(root_dir)
    adjust_api_path         = '{0}/Adjust/Adjust.m'.format(srcdir)

    # ------------------------------------------------------------------
    # Removing old iOS SDK binary
    debug_green('Removing old iOS SDK binary ...')
    if os.path.exists('{0}/libAdjust.a'):
        os.remove('{0}/libAdjust.a')

    # ------------------------------------------------------------------
    # Appending SDK prefix to source code
    debug_green('Appending SDK prefix to source code ...')
    replace_text_in_file(adjust_api_path,
        'self.activityHandler = [ADJAdjustFactory activityHandlerWithConfig:adjustConfig', 
        '[adjustConfig setSdkPrefix:@"{0}"];self.activityHandler = [ADJAdjustFactory activityHandlerWithConfig:adjustConfig'.format(sdk_prefix))

    # ------------------------------------------------------------------
    # Building new iOS SDK binary
    debug_green('Building new iOS SDK binary ...')
    os.chdir(srcdir)
    subprocess.call(['xcodebuild', '-target', 'AdjustStatic', '-configuration', 'Release', 'clean', 'build', '-UseModernBuildSystem=NO'])

    # ------------------------------------------------------------------
    # Removing SDK prefix from source code
    debug_green('Removing SDK prefix from source code ...')
    replace_text_in_file(adjust_api_path,
        '[adjustConfig setSdkPrefix:@"{0}"];self.activityHandler = [ADJAdjustFactory activityHandlerWithConfig:adjustConfig'.format(sdk_prefix),
        'self.activityHandler = [ADJAdjustFactory activityHandlerWithConfig:adjustConfig')

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
        if os.path.exists('{0}/libAdjustTest.a'):
            os.remove('{0}/libAdjustTest.a')

        # ------------------------------------------------------------------
        # Building new iOS SDK binary
        debug_green('Building new iOS SDK binary ...')
        os.chdir('{0}/AdjustTests/AdjustTestLibrary'.format(srcdir))
        subprocess.call(['xcodebuild', '-target', 'AdjustTestLibraryStatic', '-configuration', 'Release', 'clean', 'build', '-UseModernBuildSystem=NO'])
        copy_file(test_static_framework + '/Versions/A/AdjustTestLibrary', test_lib_out_dir + '/libAdjustTest.a')
