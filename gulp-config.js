module.exports = function () {
    var instanceRoot = "C:\\inetpub\\wwwroot\\dev.aienterprise.com";
    var config = {
        websiteRoot: "C:\\inetpub\\wwwroot\\dev.aienterprise.com",
        websiteUrl:"http://dev.aienterprise.com/",
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
