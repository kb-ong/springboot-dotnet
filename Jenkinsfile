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
        DOCKER_CREDS = credentials('jenkins_docker_token')	
        DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1
    }  

    stages{       
        stage('Compiling') {
          steps {		    
            dotnetBuild project: '.', sdk: 'dotnetsdk'
          }
        }
        stage('Run Test') {
          steps {		    
            dotnetTest project: '.', sdk: 'dotnetsdk'
          }
        }		
        stage('Packaging') {
          steps {		    
            dotnetPublish outputDirectory: 'app/', sdk: 'dotnetsdk', selfContained: false
          }
        }
        stage('Build image') {
          steps {           
            script{                
                withDockerRegistry(credentialsId: 'jenkins_docker_token', toolName: 'docker', url: '') {                  
                    dockerImage = docker.build registry + ":latest"
                }
            }
          }
        }
    }
}