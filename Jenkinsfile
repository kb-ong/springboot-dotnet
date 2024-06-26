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
		//PATH= "/var/jenkins_home/.dotnet/tools/:$PATH"
		PATH= "/root/.dotnet/tools/:$PATH"
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
			dotnetPack project: '.', sdk: 'dotnetsdk'
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
		     sh 'dotnet tool install --global coverlet.console'
			 sh 'dotnet tool install --global dotnet-sonarscanner'			 
             sh 'dotnet sonarscanner begin /k:"springboot-dotnet" /d:sonar.host.url="${SONARQUBE_HOST}" /d:sonar.login="${SONARQUBE_TOKEN}" /d:sonar.cs.opencover.reportsPaths=coverage.xml'
			 sh 'dotnet build --no-incremental'
			 sh 'coverlet ./HelloWorldNTest/bin/Debug/net8.0/HelloWorldNTest.dll --target "dotnet" --targetargs "test --no-build" -f=opencover -o="coverage.xml" --exclude-by-file "**/program.cs"'
			 sh 'dotnet sonarscanner end /d:sonar.login="${SONARQUBE_TOKEN}"'
          }
        }		
        stage('Push image') {
          steps {           
            script{                
                withDockerRegistry(credentialsId: 'jenkins_docker_token', toolName: 'docker', url: '') {                    
                    dockerImage.push()
                }
            }
          }
        }    
        stage('Publish Artifact') {
          steps {           
			 dotnetNuGetPush apiKeyId: 'nexus-api-key', root: 'HelloWorld/bin/Release/HelloWorld.0.0.1.nupkg', sdk: 'dotnetsdk', source: 'http://3.107.48.153:8083/repository/nuget-hosted/'
          }
        }
        stage('Deploying image to k8s') {
          steps {
              withKubeConfig(credentialsId: 'kubernetes_token', clusterName: 'aks-mylife-uat-az1-poc1', contextName: 'aks-mylife-uat-az1-poc1', namespace: '', restrictKubeConfigAccess: false, serverUrl: 'https://aks-mylife-uat-az1-poc1-dns-ror8tpjt.hcp.southeastasia.azmk8s.io:443') {
                 sh 'curl -LO "https://dl.k8s.io/release/$(curl -L -s https://dl.k8s.io/release/stable.txt)/bin/linux/amd64/kubectl"'
                 sh 'chmod u+x ./kubectl'
                 sh './kubectl apply -f deployment.yaml -n default'
              }
          }
        }
    }
	post {
		always {
			archiveArtifacts artifacts: 'trivy.txt', fingerprint: true
			publishHTML(
				[allowMissing: false, 
				alwaysLinkToLastBuild: false, 
				keepAll: false, 
				reportDir: '.', 
				reportFiles: 'trivy.txt', 
				reportName: 'TrivyReport', 
				reportTitles: '', 
				useWrapperFileDirectly: true]
			)
		}
	}	
}