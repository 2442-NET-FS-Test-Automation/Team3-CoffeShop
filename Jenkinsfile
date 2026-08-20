// Library API pipeline (quickstart shape): test (the gate), build the image, push it to
// the registry. Runs on the Windows agent. Read by Jenkins FROM this repo (Pipeline script
// from SCM) - Jenkins has already checked the repo out to read this file, so there is no
// Checkout stage.
pipeline {
    agent { label 'windows' }

    environment {
        APP_DIR  = '.'   // monorepo: the folder holding the .slnx
        REGISTRY = 'coffeapi0818.azurecr.io'           // the ACR login server
        IMAGE    = "${REGISTRY}/coffeshop-api"
        Jwt__Key = credentials('jwt-key')                // the API's JWT key - a clean clone has no dev settings; masked in the log
    }

    stages {
        stage('Test') {
            // The gate: no green tests, no image.
            steps {
                dir(env.APP_DIR) {
                    bat 'docker start coffeeshop-sqlserver'
                    powershell 'Remove-Item -Recurse -Force tests/*/TestResults -ErrorAction SilentlyContinue'
                    bat 'dotnet test CoffeShop.slnx --logger trx'
                }
            }
        }

        stage('Build image') {
            steps {
                dir(env.APP_DIR) {
                    bat 'docker build -t %IMAGE%:%BUILD_NUMBER% -t %IMAGE%:latest -f CoffeShop.Controllers/Dockerfile .'
                }
            }
        }

        stage('Push to ACR') {
            steps {
                // The credential never appears in the log: Jenkins injects it as env vars and masks them.
                withCredentials([usernamePassword(credentialsId: 'admin_credentials', usernameVariable: 'ACR_USER', passwordVariable: 'ACR_PASS')]) {
                    bat 'echo %ACR_PASS%| docker login %REGISTRY% -u %ACR_USER% --password-stdin'
                    bat 'docker push %IMAGE%:%BUILD_NUMBER%'
                    bat 'docker push %IMAGE%:latest'
                    bat 'docker logout %REGISTRY%'
                }
            }
        }
    }

    post {
        always {
            dir(env.APP_DIR) {
                archiveArtifacts allowEmptyArchive: true, artifacts: 'tests/**/TestResults/*.trx'
            }
        }
    }
}