def nuget_path = env.NugetPath
def nuget_server = env.HqvNugetServer
def nuget_server_key = env.HqvNugetServerKey
 
stage('compile') {
    node('windows') {
        checkout scm //git 'https://github.com/hqv1/media-tools.git'
        bat 'dotnet clean'
        bat 'dotnet restore'
        bat 'dotnet build -c Release'
        stash 'everything'
    }
}

stage('test') {
    node('windows') {
        unstash 'everything'
        dir("ThumbnailSheet.Tests") {
            bat 'dotnet restore'
            bat 'dotnet test --filter Category=Unit'
        }
        dir("VideoFileInfo.Tests") {
            bat 'dotnet restore'
            bat 'dotnet test --filter Category=Unit'
        }
    }
}

stage('publish') {
    node('windows') {
        unstash 'everything'
        bat 'del /S *.nupkg'
        dir("Types") {
            bat 'dotnet pack --no-build -c Release'
        }
        dir("ThumbnailSheet") {
            bat 'dotnet pack --no-build -c Release'
        }
        dir("VideoFileInfo") {
            bat 'dotnet pack --no-build -c Release'
        }
        bat "${nuget_path} push **\\*.nupkg ${nuget_server_key} -Source ${nuget_server}"
        archiveArtifacts '**\\*.nupkg'
    }
}