locals {
  bucket_name = "movie-recognizer"
}

resource "yandex_storage_bucket" "bucket" {
  bucket     = local.bucket_name
  access_key = var.deployer_access_key
  secret_key = var.deployer_secret_key

  grant {
    id          = module.process-video-handler.service_account_id
    type        = "CanonicalUser"
    permissions = ["READ", "WRITE"]
  }

  anonymous_access_flags {
    read = true
    list = false
  }
}