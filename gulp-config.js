module.exports = function () {
    var instanceRoot = "C:\\AIE_Content";
    var config = {
        websiteRoot: "C:\\AIE_deploy",
        UnicornDumpTemp: "C:\\AIE_deploy\\Unicorn",
        websiteUrl: "http://dev.aienterprise.com/",
        siteHostName: "https://10.10.23.155/",
        deployTemp: "C:\\AIE_deploy",
        solutionName: "AIEnterprise",
        buildConfiguration: "Debug",
        buildToolsVersion: 15.0,
        buildMaxCpuCount: 0,
        buildVerbosity: "minimal",
        buildPlatform: "Any CPU",
        publishPlatform: "AnyCpu",
        runCleanBuilds: false
    };
    return config;
}
