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
        stage('Trivy Scanning') {
           steps {
              sh 'curl -sfL https://raw.githubusercontent.com/aquasecurity/trivy/main/contrib/install.sh |sh -s -- -b /usr/local/bin v0.51.2'             
              sh 'trivy image --debug --scanners vuln --timeout 30m --no-progress -o trivy.txt ${registry}:latest'
           }
        }		
        stage('SonarQube Analysis') {
          steps {		    
             sh 'dotnet sonarscanner begin /k:"springboot-dotnet" /d:sonar.host.url="${SONARQUBE_HOST}" /d:sonar.login="${SONARQUBE_TOKEN}" /d:sonar.cs.opencover.reportsPaths=coverage.xml'
			 sh 'dotnet build --no-incremental'
			 sh 'coverlet ./HelloWorldNTest/bin/Debug/net8.0/HelloWorldNTest.dll --target "dotnet" --targetargs "test --no-build" -f=opencover -o="coverage.xml" --exclude-by-file "**/program.cs"'
			 sh 'dotnet sonarscanner end /d:sonar.login="${SONARQUBE_TOKEN}"'
          }
        }		
    }
}