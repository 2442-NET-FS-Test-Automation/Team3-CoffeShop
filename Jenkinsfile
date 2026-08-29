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
            environment{
                JWT__KEY = credentials('coffeeshop-jwt-key')
            }
            steps {
                dir(env.APP_DIR){
                    bat 'docker start coffeeshop-sqlserver'
                    powershell 'Remove-Item -Recurse -Force CoffeShop.Test/*/TestResults -ErrorAction SilentlyContinue'
                    bat 'dotnet test CoffeShop.Test/UnitTest.Tests/UnitTest.Tests.csproj -c Release --no-build --logger trx --logger junit'
                    bat 'dotnet test CoffeShop.Test/IntegrationTest.Test/IntegrationTest.Test.csproj -c Release --no-build --logger trx --logger junit'
                }
            }
        }
        stage('Docker Build'){
            steps{
                dir(env.APP_DIR){
                    bat 'docker build -t coffeshop-api:%BUILD_NUMBER% -f CoffeShop.Controllers/Dockerfile .'
                }
            }
        }
    }
    post {
        always {
            dir(env.APP_DIR){
                junit testResults: 'CoffeShop.Test/**/TestResults/*.xml'
                archiveArtifacts allowEmptyArchive: true, artifacts: 'CoffeShop.Test/**/TestResults/*.trx'
            }
        }
    }
}