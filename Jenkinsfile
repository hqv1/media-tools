def nuget_path = env.NugetPath
def nuget_server = env.HqvNugetServer
def nuget_server_key = env.HqvNugetServerKey

node('windows') {
	stage('compile') {    
		checkout scm //git 'https://github.com/hqv1/media-tools.git'
		bat 'dotnet clean'
		bat 'dotnet restore'     			
		bat 'dotnet build -c Release' 
	}
	
	stage('test') {    
		dir("test/Thumbnail.Tests") {
		    bat 'dotnet restore'
		    bat 'dotnet test --filter Category=Unit'
		}
		dir("test/ThumbnailSheet.Tests") {
		    bat 'dotnet restore'
		    bat 'dotnet test --filter Category=Unit'
		}
		dir("test/VideoFileInfo.Tests") {
		    bat 'dotnet restore'
		    bat 'dotnet test --filter Category=Unit'
		}
	}

	stage('publish') {
		bat 'del /S *.nupkg'
		dir("Types") {
		    bat 'dotnet pack --no-build -c Release'
		}
		dir("FileDownload") {
		    bat 'dotnet pack --no-build -c Release'
		}
		dir("Thumbnail") {
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


/*
stage('publish_framework') {
    node('windows') {
        unstash 'everything'                
		dir("ThumbnailSheet.Framework") {
            bat "${nuget_path} pack ThumbnailSheet.Framework.csproj -Prop Configuration=Release"
			bat "${nuget_path} push **\\Hqv.MediaTools.ThumbnailSheet.Framework.*.nupkg ${nuget_server_key} -Source ${nuget_server}"
			archiveArtifacts '**\\*.nupkg'
        }                
    }
}
*/
