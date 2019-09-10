module.exports = function () {
    var instanceRoot = "C:\\inetpub\\wwwroot\\AIECorporate-cms.com";
    var config = {
        websiteRoot: "C:\\inetpub\\wwwroot\\AIECorporate-cms.com",
        deployTemp: "C:\AIE_Content",
        UnicornDumpTemp: "C:\\AIE_deploy\\Unicorn",
        websiteUrl: "http://dev.aienterprise.com/",
        siteHostName: "https://10.10.23.155/",
        solutionName: "AIEnterprise",
        buildConfiguration: "Release",
        buildToolsVersion: 15.0,
        buildMaxCpuCount: 0,
        buildVerbosity: "minimal",
        buildPlatform: "Any CPU",
        publishPlatform: "AnyCpu",
        runCleanBuilds: false
    };
    return config;
}
