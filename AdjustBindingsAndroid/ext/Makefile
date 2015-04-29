MVNDIR = ./sdk/Adjust
JARINDIR = ./sdk/Adjust/target
JAROUTDIR = ../Jars

default: copy

build: clean
	cd $(MVNDIR); mvn package

clean:
	cd $(MVNDIR); mvn clean

copy: build
	cp $(JARINDIR)/adjust-android-*.*.*.jar $(JAROUTDIR); rm $(JAROUTDIR)/*-javadoc.jar; rm $(JAROUTDIR)/*-sources.jar
