from scripting_utils import *

def build(version, root_dir, android_submodule_dir, with_test_lib):
    # ------------------------------------------------------------------
    # paths
    sdk_adjust_dir  = '{0}/ext/android/sdk'.format(root_dir)
    jar_in_dir      = '{0}/Adjust/sdk-core/build/libs'.format(sdk_adjust_dir)
    jar_out_dir     = '{0}/android/AdjustSdk.Xamarin.Android/Jars'.format(root_dir)
    project_dir     = '{0}/ext/android/sdk/Adjust'.format(root_dir)
    adjust_api_path = '{0}/Adjust/sdk-core/src/main/java/com/adjust/sdk/Adjust.java'.format(sdk_adjust_dir)

    # ------------------------------------------------------------------
    # Appending SDK prefix to source code ...
    debug_green('Appending SDK prefix to source code ...')
    replace_text_in_file(adjust_api_path,
        'adjustInstance.onCreate(adjustConfig);', 
        'adjustConfig.setSdkPrefix("xamarin{0}");adjustInstance.onCreate(adjustConfig);'.format(version))
    replace_text_in_file(adjust_api_path,
        'return adjustInstance.getSdkVersion();', 
        'return "xamarin{0}@" + adjustInstance.getSdkVersion();'.format(version))

    # ------------------------------------------------------------------
    # Running Gradle tasks: clean makeReleaseJar ...
    debug_green('Running Gradle tasks: clean makeReleaseJar ...')
    change_dir(project_dir)
    gradle_make_release_jar()

    # ------------------------------------------------------------------
    # Removing SDK prefix from source code ...
    replace_text_in_file(adjust_api_path,
        'adjustConfig.setSdkPrefix("xamarin{0}");adjustInstance.onCreate(adjustConfig);'.format(version),
        'adjustInstance.onCreate(adjustConfig);')
    replace_text_in_file(adjust_api_path,
        'return "xamarin{0}@" + adjustInstance.getSdkVersion();'.format(version),
        'return adjustInstance.getSdkVersion();')

    # ------------------------------------------------------------------
    # Moving the generated Android SDK JAR from ${JAR_IN_DIR} to ${JAR_OUT_DIR} ...
    debug_green('Moving the generated Android SDK JAR from {0} to {1} ...'.format(jar_in_dir, jar_out_dir))
    copy_file('{0}/adjust-sdk-release.jar'.format(jar_in_dir), '{0}/adjust-android.jar'.format(jar_out_dir))

    if with_test_lib:
        # ------------------------------------------------------------------
        # Test Library paths
        set_log_tag('ANROID-TEST-LIB-BUILD')
        waiting_animation(duration=4.0, step=0.025)
        debug_green('Building Test Library started ...')
        test_jar_in_dir  = '{0}/Adjust/test-library/build/libs'.format(sdk_adjust_dir)
        test_jar_out_dir = '{0}/android/Test/TestLib/Jars'.format(root_dir)

        # ------------------------------------------------------------------
        # Running Gradle task: test-library:adjustMakeJarRelease ...
        debug_green('Running Gradle task: test-library:adjustMakeJarRelease ...')
        change_dir(project_dir)
        gradle_run([':test-library:adjustMakeJarRelease'])

        # ------------------------------------------------------------------
        # Moving the generated Android SDK JAR from jar in to jar out dir ...
        debug_green('Moving the generated Android SDK JAR from {0} to {1} dir ...'.format(test_jar_in_dir, test_jar_out_dir))
        copy_file('{0}/test-library-release.jar'.format(test_jar_in_dir), '{0}/adjust-testing.jar'.format(test_jar_out_dir))
