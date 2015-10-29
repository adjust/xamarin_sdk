SRCDIR = ./sdk
LIBOUTDIR = ..

default: copy

build:	
	cd $(SRCDIR); \
	xcodebuild -target framework -configuration Release

copy: build
	rm -rf $(LIBOUTDIR)/libAdjust.a; \
	cp -R $(SRCDIR)/Adjust.framework/Versions/A/Adjust $(LIBOUTDIR); \
	mv $(LIBOUTDIR)/Adjust $(LIBOUTDIR)/libAdjust.a; \
	rm -rf $(SRCDIR)/Adjust.framework;
