module.exports = function () {
    var instanceRoot = "C:\\inetpub\\wwwroot\\AIECorporate-cms.com";
    var config = {
        websiteRoot: "C:\\Package\\AIE_deploy\\WebSite",
        deployTemp: "C:\\Package\\AIE_deploy\\WebSite",
        UnicornDumpTemp: "C:\\Package\\AIE_deploy\\Unicorn",
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
