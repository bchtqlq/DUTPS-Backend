pipeline {
    agent any
    stages {
        stage("Clear") {
            steps {
                script {
                    try {
                        sh "docker rm  dotnet_parking -f"
                        sh "docker rmi dotnet_parking_api -f"
                        sh 'docker rm /$(docker ps --filter status=exited -q)'
                    }
                    catch (err) {
                        echo err.getMessage()
                    }
                }
            }
        }
        stage('Build Stage') {
            steps {
                sh 'cd DUTPS.API'
                sh 'dotnet build'
                
            }
        }
        stage('Test Stage') {
            steps {
                sh 'cd DUTPS.API.Test'
                sh 'dotnet test'
            }
        }
        stage("Release Stage and Deploy Stage") {
            steps {
                sh 'docker-compose up --build -d'
            }
        }
    }
}
