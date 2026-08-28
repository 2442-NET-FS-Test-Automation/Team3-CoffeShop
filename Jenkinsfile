pipeline {
    agent { label 'windows'}
    environment {
        APP_DIR = '.'
        REGISTRY = 'coffeapi0818.azurecr.io'
        IMAGE = "${REGISTRY}/coffeshop-api"
    }
    stages {
        stage('Build'){
            steps{
                dir(env.APP_DIR){
                    bat 'dotnet build CoffeShop.slnx -c Release'
                }
            }
        }
        stage('Test'){
            steps {
                dir(env.APP_DIR){
                    bat 'docker start coffeeshop-sqlserver'
                    powershell 'Remove-Item -Recurse -Force CoffeShop.Test/*/TestResults -ErrorAction SilentlyContinue'
                    bat 'dotnet test CoffeShop.slnx -c Release --no-build --logger trx'
                }
            }
        }
    }
    post {
        always {
            dir(env.APP_DIR){
                archiveArtifacts allowEmptyArchive: true, artifacts: 'CoffeShop.Test/**/TestResults/*.trx'
            }
        }
    }
}