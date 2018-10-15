import os, subprocess
from scripting_utils import *

def build(sdk_prefix, root_dir, android_submodule_dir, with_test_lib):
    # ------------------------------------------------------------------
    # paths
    sdk_adjust_dir  = '{0}/ext/android/sdk'.format(root_dir)
    jar_in_dir      = '{0}/Adjust/adjust/build/intermediates/intermediate-jars/release'.format(sdk_adjust_dir)
    jar_out_dir     = '{0}/android/AdjustSdk.Xamarin.Android/Jars'.format(root_dir)
    project_dir     = '{0}/ext/android/sdk/Adjust'.format(root_dir)
    adjust_api_path = '{0}/Adjust/adjust/src/main/java/com/adjust/sdk/Adjust.java'.format(sdk_adjust_dir)

    # ------------------------------------------------------------------
    # Appending SDK prefix to source code ...
    debug_green('Appending SDK prefix to source code ...')
    replace_text_in_file(adjust_api_path,
        'adjustInstance.onCreate(adjustConfig);', 
        'adjustConfig.setSdkPrefix("{0}");adjustInstance.onCreate(adjustConfig);'.format(sdk_prefix))

    # ------------------------------------------------------------------
    # Running Gradle tasks: clean makeReleaseJar ...
    debug_green('Running Gradle tasks: clean makeReleaseJar ...')
    os.chdir(project_dir)
    subprocess.call(['./gradlew', 'clean', 'makeReleaseJar'])
    # flag which detects building debug jar instead ?
    # subprocess.call(['./gradlew', 'clean', 'makeDebugJar'])

    # ------------------------------------------------------------------
    # Removing SDK prefix from source code ...
    replace_text_in_file(adjust_api_path,
        'adjustConfig.setSdkPrefix("{0}");adjustInstance.onCreate(adjustConfig);'.format(sdk_prefix),
        'adjustInstance.onCreate(adjustConfig);')

    # ------------------------------------------------------------------
    # Moving the generated Android SDK JAR from ${JAR_IN_DIR} to ${JAR_OUT_DIR} ...
    copy_files('classes.jar', jar_in_dir, jar_out_dir)
    rename_file('classes.jar', 'adjust-android.jar', jar_out_dir)

    if with_test_lib:
        # ------------------------------------------------------------------
        # Test Library paths
        set_log_tag('ANROID-TEST-LIB-BUILD')
        waiting_animation(duration=4.0, step=0.025)
        debug_green('Building Test Library started ...')
        test_jar_in_dir  = '{0}/Adjust/testlibrary/build/intermediates/intermediate-jars/release'.format(sdk_adjust_dir)
        test_jar_out_dir = '{0}/android/Test/TestLib/Jars'.format(root_dir)

        # ------------------------------------------------------------------
        # Running Gradle tasks: clean testlibrary:makeJar ...
        debug_green('Running Gradle tasks: clean testlibrary:makeJar ...')
        os.chdir(project_dir)
        subprocess.call(['./gradlew', 'clean', ':testlibrary:makeJar'])

        # ------------------------------------------------------------------
        # Moving the generated Android SDK JAR from jar in to jar out dir ...
        debug_green('Moving the generated Android SDK JAR from jar in to jar out dir ...')
        copy_files('classes.jar', test_jar_in_dir, test_jar_out_dir)
        rename_file('classes.jar', 'adjust-testing.jar', test_jar_out_dir)
