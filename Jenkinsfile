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
		PATH= "/var/jenkins_home/.dotnet/tools/"
    }  

    stages{       
        stage('Compiling') {
          steps {		    
            //sh 'dotnet --version'
			//sh 'export PATH=/var/jenkins_home/.dotnet/tools/dotnet-sonarscanner:$PATH'
			//sh 'dotnet /var/jenkins_home/.dotnet/tools/dotnet-sonarscanner'
			sh 'echo $PATH'
			sh 'dotnet-sonarscanner'
          }
        }
		
    }
}