module.exports = function () {
    var instanceRoot = "C:\\inetpub\\wwwroot\\nes.local";
    var config = {
       // websiteRoot: "C:\\inetpub\\wwwroot\\nes.local",// Please update your location here as well
        websiteRoot: "C:\\Nes_deploy\\WebSite",
        deployTemp: "C:\\Nes_deploy\\WebSite",
        UnicornDumpTemp: "C:\\Nes_deploy\\Unicorn",		
        solutionName: "NES",
        buildConfiguration: "DEV",
        buildToolsVersion: 15.0,
        buildMaxCpuCount: 0,
        buildVerbosity: "minimal",
        buildPlatform: "Any CPU",
        publishPlatform: "AnyCpu",
        runCleanBuilds: false
    };
    return config;
}
