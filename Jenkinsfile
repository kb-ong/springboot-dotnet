pipeline{
    //agent {
    //    node {
    //        label 'java_jenkins'
    //    }
    //}
	agent any
    tools {
         dockerTool 'docker'
         maven 'Maven.3.9.6' 
         jdk 'JDK22'
         git 'git'
		 dotnetsdk 'dotnetsdk'
    }
    environment {
        registry = "riko20xx/springboot-dotnet"
        dockerImage = ""
        DOCKER_HOST = "tcp://3.107.48.153:8443"
		SONARQUBE_HOST = "http://3.107.48.153:9000"
        DOCKER_CREDS = credentials('jenkins_docker_token')	
        DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1
		PATH= "/var/jenkins_home/.dotnet/tools/:$PATH"
    }  

    stages{       
        stage('Compiling') {
          steps {		    
             sh 'dotnet sonarscanner begin /k:"springboot-dotnet" /d:sonar.host.url="${SONARQUBE_HOST}" /d:sonar.login="squ_8de77b2385717e4ebe1160b10d62f50a83055406" /d:sonar.cs.opencover.reportsPaths=coverage.xml'
			 sh 'dotnet build --no-incremental'
			 sh 'coverlet ./HelloWorldNTest/bin/Debug/net8.0/HelloWorldNTest.dll --target "dotnet" --targetargs "test --no-build" -f=opencover -o="coverage.xml" --exclude-by-file "**/program.cs"'
			 sh 'dotnet sonarscanner end /d:sonar.login="squ_8de77b2385717e4ebe1160b10d62f50a83055406"'
          }
        }


		
    }
}