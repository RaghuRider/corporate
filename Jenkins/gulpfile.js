/// <binding />
var gulp = require("gulp");
var msbuild = require("gulp-msbuild");
var debug = require("gulp-debug");
var foreach = require("gulp-foreach");
var rename = require("gulp-rename");
var newer = require("gulp-newer");
var util = require("gulp-util");
//var unicorn = require("./scripts/unicorn.js");
//var habitat = require("./scripts/habitat.js");
var runSequence = require("run-sequence");
var nugetRestore = require('gulp-nuget-restore');
var fs = require('fs');
var yargs = require("yargs").argv;
var robocopy = require("robocopy");
var exec = require('gulp-exec');
var mkdirp = require('mkdirp');
var del = require('del');
var glob = require("glob");

var config;

config = require("./gulp-config.js")();

module.exports.config = config;

var publishProject = function (location, dest) {
    dest = dest || config.deployTemp;

    console.log("publish to " + dest + " folder");
    return gulp.src(["./src/" + location + "/code/*.csproj"])
        .pipe(foreach(function (stream, file) {
            return publishStream(stream, dest);
        }));
}

var publishProjects = function (location, dest) {
    dest = dest || config.deployTemp;

    console.log("publish to " + dest + " folder");
    return gulp.src([location + "/**/code/*.csproj"])
        .pipe(foreach(function (stream, file) {
            return publishStream(stream, dest);
        }));
};

gulp.task("Build-Solution",
    function () {
        var targets = ["Build"];
        if (config.runCleanBuilds) {
            targets = ["Clean", "Build"];
        }

        var solution = "./" + config.solutionName + ".sln";
        return gulp.src(solution)
            .pipe(msbuild({
                targets: targets,
                configuration: config.buildConfiguration,
                logCommand: false,
                verbosity: config.buildVerbosity,
                stdout: true,
                errorOnFail: true,
                maxcpucount: config.buildMaxCpuCount,
                nodeReuse: false,
                toolsVersion: config.buildToolsVersion,
                properties: {
                    Platform: config.buildPlatform
                }
            }));
    });

/*****************************
  Initial setup
*****************************/

gulp.task("Nuget-Restore",
    function (callback) {
        var solution = "./" + config.solutionName + ".sln";
        return gulp.src(solution).pipe(nugetRestore());
    });



/*****************************
  Publish
*****************************/
var publishStream = function (stream, dest) {
    var targets = ["Build"];

    return stream
        .pipe(debug({
            title: "Building project:"
        }))
        .pipe(msbuild({
            targets: targets,
            configuration: config.buildConfiguration,
            logCommand: false,
            verbosity: config.buildVerbosity,
            stdout: true,
            errorOnFail: true,
            maxcpucount: config.buildMaxCpuCount,
            nodeReuse: false,
            toolsVersion: config.buildToolsVersion,
            properties: {
                Platform: config.publishPlatform,
                DeployOnBuild: "true",
                DeployDefaultTarget: "WebPublish",
                WebPublishMethod: "FileSystem",
                BuildProjectReferences: "false",
                DeleteExistingFiles: "false",
                publishUrl: dest
            },
            customArgs: ["/restore"]
        }));
};

var publishProjects = function (location, dest) {
    dest = dest || config.deployTemp;

    console.log("publish to " + dest + " folder");
    return gulp.src([location + "/**/code/*.csproj"])
        .pipe(foreach(function (stream, file) {
            return publishStream(stream, dest);
        }));
};

gulp.task("Publish-Foundation-Projects", function () {
    //return publishProjects("./src/Foundation");
    return publishProjects("../src/Foundation", config.websiteRoot);
});

gulp.task("Publish-Feature-Projects", function () {
    //return publishProjects("./src/Feature");
    return publishProjects("../src/Feature", config.websiteRoot);
});

gulp.task("Publish-Project-Projects", function () {
    //return publishProjects("./src/Project");
    return publishProjects("../src/Project", config.websiteRoot);
});

//gulp.task("default", function (callback) {
//    return runSequence(
//        "01-Nuget-Restore",
//        "Build-Solution",
//        "Publish-Feature-Projects",
//        "Publish-Foundation-Projects",
//        "Publish-Project-Projects",
//        "Sync-Unicorn", callback);
//});

gulp.task("Sync-Unicorn",
    function (callback) {
        var options = {};
        options.siteHostName = options.websiteUrl; //habitat.getSiteUrl();

        options.authenticationConfigFile = config.websiteRoot + "/App_config/Include/Unicorn/Unicorn.zSharedSecret.config";
        options.maxBuffer = Infinity;

        unicorn(function () {
            return callback()
        }, options);
    });

gulp.task("02-Publish-Feature-Projects", function (callback) {
    return runSequence(
        "CreateTempFolder",
        "Nuget-Restore",
        "Publish-Feature-Projects",
        //"CopyDll",
        //"CopyThirdPartyDll",
        //"Publish-All-Views",
        //"Publish-All-Configs",
        //"CopyAppConfig",
        callback);
    //"RemoveTempFolder", callback);
});

gulp.task("03-Publish-Foundation-Projects", function (callback) {
    return runSequence(
        "CreateTempFolder",
        "Nuget-Restore",
        "Publish-Foundation-Projects",
        //"CopyDll",
        //"CopyThirdPartyDll",
        //"Publish-All-Views",
        //"Publish-All-Configs",
        //"CopyAppConfig",
        //  "RemoveTempFolder",
        callback);
});

gulp.task("04-Publish-Project-Projects", function (callback) {
    return runSequence(
        "CreateTempFolder",
        "Nuget-Restore",
        "Publish-Project-Projects",
        "Sunc-Unicorn",
        //"CopyDll",
        //"Publish-All-Views",
        //"Publish-All-Configs",
        //"CopyAppConfig",
        //"RemoveTempFolder,"
        callback);
});

gulp.task("05-Publish-Complete-Solution", function (callback) {
    return runSequence(
        "02-Publish-Feature-Projects",
        "03-Publish-Foundation-Projects",
        "04-Publish-Project-Projects",
		"Publish-All-Unicorn",
		callback);
});

gulp.task('CopyDll', function () {
    return robocopy({
        source: config.deployTemp + '/bin',
        destination: config.websiteRoot + '/bin',
        files: ['NES.NESPower.*.dll'],
        copy: {
            subdirs: true,
            mirror: false,
            emptySubdirs: false
        },
        retry: {
            count: 2,
            wait: 3
        }
    });
});

gulp.task('CopyThirdPartyDll', function () {
    return robocopy({
        source: config.deployTemp + '/bin',
        destination: config.websiteRoot + '/bin',
        files: ['Glass.*.dll', 'Unicorn.dll', 'Unicorn.*.dll', 'Rainbow.*.dll', 'Configy.dll', 'Kamsar.WebConsole.dll', 'MicroCHAP.dll', 'EntityFramework*'],
        copy: {
            subdirs: true,
            mirror: false,
            emptySubdirs: false
        },
        retry: {
            count: 2,
            wait: 3
        }
    });
});

//gulp.task('CopyViews', function () {
//    return robocopy({
//        source: config.deployTemp + '/Views',
//        destination: config.websiteRoot + '/Views',
//        files: ['*.cshtml'],
//        copy: {
//            subdirs: true,
//            mirror: false,
//            emptySubdirs: false
//        },
//        retry: {
//            count: 2,
//            wait: 3
//        }
//    });
//});

gulp.task('CopyAppConfig', function () {
    return robocopy({        
        source: config.deployTemp + '/App_Config',        
        destination: config.websiteRoot + '/App_Config',
        files: ['*.config'],
        copy: {
            subdirs: true,
            mirror: false,
            emptySubdirs: false
        },
        retry: {
            count: 2,
            wait: 3
        }
    });
});

gulp.task("Publish-All-Configs",
    function () {
        var root = "./src";
        var roots = [root + "/**/App_Config", "!" + root + "/**/tests/App_Config", "!" + root + "/**/obj/**/App_Config"];
        var files = "/**/*.config";
        var destination = config.deployTemp + "\\App_Config";
        return gulp.src(roots, { base: root }).pipe(
            foreach(function (stream, file) {
                console.log("Publishing from " + file.path);
                gulp.src(file.path + files, { base: file.path })
                    .pipe(newer(destination))
                    .pipe(debug({ title: "Copying " }))
                    .pipe(gulp.dest(destination));
                return stream;
            })
        );
    });

gulp.task("Publish-All-Views",
    function () {
        var root = "./src";
        var roots = [root + "/**/Views", "!" + root + "/**/obj/**/Views"];
        var files = "/**/*.cshtml";
        var destination = config.deployTemp + "\\Views";
        return gulp.src(roots, { base: root }).pipe(
            foreach(function (stream, file) {
                console.log("Publishing from " + file.path);
                gulp.src(file.path + files, { base: file.path })
                    .pipe(newer(destination))
                    .pipe(debug({ title: "Copying " }))
                    .pipe(gulp.dest(destination));
                return stream;
            })
        );
    });

gulp.task('CreateSiteFolder', function () {
    return mkdirp(config.websiteRoot, function (err) {
        if (err) console.error(err)
        else console.log('Website directory created.')
    });
});

gulp.task('CreateTempFolder', function () {
    return mkdirp(config.deployTemp, function (err) {
        if (err) console.error(err)
        else console.log('Temp folder created.')
    })
});

gulp.task('RemoveTempFolder', function () {
    return del([config.deployTemp], { force: true }).then(paths => {
        console.log('Deleted files and folders:\n', paths.join('\n'));
    });
});

gulp.task('RemoveUnicornFolder', function () {
    return del([config.UnicornDumpTemp], { force: true }).then(paths => {
        console.log('Deleted files and folders:\n', paths.join('\n'));
    });
});

gulp.task("Publish-All-Unicorn", function (callback) {
    return runSequence(
		"RemoveUnicornFolder",
        "Publish-Feature-Unicorn",
        "Publish-Foundation-Unicorn",
        "Publish-Project-Unicorn", callback);
});

gulp.task("Publish-Foundation-Unicorn",
    function () {
        return robocopy({
            //roots = [root + "/**/serialization"],
            source: ["../src" + "/Foundation/Serialization" + "/serialization"],
            destination: config.UnicornDumpTemp + "/Foundation/Serialization" + "/serialization",
            files: ['*.yml'],
            copy: {
                subdirs: true,
                mirror: true,
                emptySubdirs: false
            },
            retry: {
                count: 2,
                wait: 3
            }
        });
    });

gulp.task("Publish-Project-Unicorn",
    function () {
        return robocopy({
            //roots = [root + "/**/serialization"],
            source: ["../src" + "/Project/NES" + "/serialization"],
            destination: config.UnicornDumpTemp + "/Project/NES" + "/serialization",
            files: ['*.yml'],
            copy: {
                subdirs: true,
                mirror: true,
                emptySubdirs: false
            },
            retry: {
                count: 2,
                wait: 3
            }
        });
    });

gulp.task("Publish-Feature-Unicorn",
    function () {
        var root = "../src";
        var roots = ["../src"+ "/Feature/**/"+ "/**/serialization"];
        var files = "/**/*.yml";
        //var destination = config.websiteRoot + "\\Unicorn";
        return gulp.src(roots, { base: root }).pipe(
            foreach(function (stream, file) {
                //console.log("Lastindex of \ " + file.path.lastIndexOf("\\"));
                //console.log("Publishing from " + file.path.substr(0,file.path.lastIndexOf("\\")));
				var newSubPath = file.path.substr(0,file.path.lastIndexOf("\\"));
				var dynamicFolder = newSubPath.substr(newSubPath.lastIndexOf("\\")+1);				
				//console.log("newSubPath " + newSubPath);
				//console.log("dynamicFolder " + dynamicFolder);
                gulp.src(file.path + files, { base: file.path })
                    //.pipe(newer(destination))
                    .pipe(debug({ title: "Copying " }))
					//.pipe(debug({ title: "Destination " +config.websiteRoot + "\\Unicorn" +"\\Feature\\" +dynamicFolder + "\\serialization" }))
                    .pipe(gulp.dest(config.UnicornDumpTemp + "\\Feature\\" +dynamicFolder + "\\serialization" ));
                return stream;
            })
        );
    });