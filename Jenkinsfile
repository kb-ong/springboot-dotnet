pipeline{
    agent {
        node {
            label 'java_jenkins'
        }
    }
	//agent any
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
		SONARQUBE_TOKEN = credentials('sonarqube-token')	
        DOCKER_CREDS = credentials('jenkins_docker_token')
        DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1
		PATH= "/var/jenkins_home/.dotnet/tools/:$PATH"
    }
    stages{	
        stage('SonarQube Analysis') {
          steps {
		     sh 'dotnet tool install --global coverlet.console'
			 sh 'dotnet tool install --global dotnet-sonarscanner'
			 sh 'ls -la /root/.dotnet/tools'
          }
        }		
    }
}