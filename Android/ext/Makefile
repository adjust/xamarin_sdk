MVNDIR = ./sdk/Adjust
JARINDIR = ./sdk/Adjust/target
JAROUTDIR = ../AdjustSdk.Xamarin.Android/Jars

default: copy

build: clean
	cd $(MVNDIR); mvn package

clean:
	cd $(MVNDIR); mvn clean

copy: build
	rm -f $(JAROUTDIR)/adjust-android*; \
	cp $(JARINDIR)/adjust-android-*.*.*.jar $(JAROUTDIR); \
	rm -f $(JAROUTDIR)/*-javadoc.jar; \
	rm -f $(JAROUTDIR)/*-sources.jar; \
	mv $(JAROUTDIR)/adjust-android-*.*.*.jar $(JAROUTDIR)/adjust-android.jar
