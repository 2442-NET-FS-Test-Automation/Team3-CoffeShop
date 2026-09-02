pipeline {
    agent { label 'windows'}
    environment {
        APP_DIR = '.'
        REGISTRY = 'coffeshopignacio0828.azurecr.io'
        IMAGE = "${REGISTRY}/coffeshop-api"
        SPA_DIR = 'CoffeShop.Frontend'
        API_URL = 'https://rg-coffeshop-api-ignacio0828-eyh3drgfdcbmc8gu.centralus-01.azurewebsites.net'
        SITE_URL = 'https://coffeshopwebignacio0828.z19.web.core.windows.net'
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
        stage('Database Migration'){
            environment{
                ConnectionStrings__CoffeShop = credentials('azure-sql-connection')
            }
            steps {
                dir(env.APP_DIR){
                    bat 'dotnet ef database update --project CoffeShop.Data/CoffeShop.Data.csproj --startup-project CoffeShop.Controllers/CoffeShop.Controllers.csproj'
                }
            }
        }
        stage('Docker Build'){
            steps{
                dir(env.APP_DIR){
                    bat 'docker build -t %IMAGE%:%BUILD_NUMBER% -f CoffeShop.Controllers/Dockerfile .'
                }
            }
        }
        stage('Push to ACR') {
            steps {
                withCredentials([usernamePassword(
                    credentialsId: 'acr-coffeeshop',
                    usernameVariable: 'ACR_USERNAME',
                    passwordVariable: 'ACR_PASSWORD'
                )]) {
                    bat 'echo %ACR_PASSWORD%| docker login %REGISTRY% -u %ACR_USERNAME% --password-stdin'
                    bat 'docker push %IMAGE%:%BUILD_NUMBER%'
                    bat 'docker tag %IMAGE%:%BUILD_NUMBER% %IMAGE%:latest'
                    bat 'docker push %IMAGE%:latest'
                }
            }
        }
        stage('Build SPA') {
            steps {
                dir(env.SPA_DIR) {
                    bat 'npm ci'
                    bat 'set VITE_API_BASE_URL=%API_URL%&& npm run build'
                }
            }
        }
        stage('Publish SPA') {
            steps {
                withCredentials([string(credentialsId: 'web-sas', variable: 'SAS')]) {
                    dir(env.SPA_DIR) {
                        powershell '''
                            $dest = 'https://coffeshopwebignacio0828.blob.core.windows.net/$web' + $env:SAS
                            azcopy sync dist $dest --recursive --delete-destination=true
                        '''
                    }
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
