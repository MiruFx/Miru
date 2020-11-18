module.exports = {
    title: 'Miru',
    tagline: false,
    description: 'Ready to go full-stack open-source framework for developing ASP.NET Core web applications',
    markdown: {
        toc: {
            includeLevel: [1,2,3] 
        },
        config: md => {
            md.use(require('markdown-it-vuepress-code-snippet-enhanced'))
        }
    },
    plugins: [
        [
          '@vuepress/google-analytics',
          {
            'ga': 'UA-56632357-4' // UA-00000000-0
          }
        ]
      ],
    themeConfig: {
        sidebarDepth: 0,
        displayAllHeaders: true,
        nav: [{
            text: 'Home',
            link: '/'
        },
        {
            text: 'Github',
            link: 'https://www.github.com/mirufx/miru'
        }],
        sidebar: [{
                title: 'Introduction',
                collapsable: false,
                children: [
                    'Introduction/GettingStarted',
                    'Introduction/FirstLook',
                    'Introduction/SolutionOrganization',
                    'Introduction/Configuration'
                ],
            },
            {
                title: 'Features',
                collapsable: false,
                children: [
                    'Features/Overview.md',
                    'Features/Pipeline.md',
                    'Features/Domain.md'
                ],
            },
            {
                title: 'Database',
                collapsable: false,
                children: [
                    'Database/Migrations.md',
                    'Database/EntityFramework.md'
                ],
            },
            {
                title: 'Frontend',
                collapsable: false,
                children: [
                    'Frontend/ControllersRouting.md',
                    'Frontend/ViewsHtml.md',
                    'Frontend/JavascriptCssAssets.md'
                ],
            },
            {
                title: 'Infrastructure',
                collapsable: false,
                children: [
                    'Infrastructure/CommandLine.md',
                    'Infrastructure/Queueing.md',
                    'Infrastructure/Mailing.md',
                    'Infrastructure/Logging.md'
                ],
            },
            {
                title: 'Testing',
                collapsable: false,
                children: [
                    'Testing/Overview.md',
                    'Testing/Arrange.md',
                    'Testing/DomainUnitTests.md',
                    'Testing/FeatureIntegrationTests.md',
                    'Testing/PageTests.md'
                ],
            }            
        ]
    }
}