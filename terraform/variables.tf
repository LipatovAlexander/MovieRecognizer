variable deployer_access_key {
  type      = string
  sensitive = true
}

variable deployer_secret_key {
  type      = string
  sensitive = true
}

variable folder_id {
  type = string
}

variable api_registry {
  type = string
}

variable api_repository {
  type = string
}

variable github_sha {
  type = string
}

variable function_packages_bucket {
  type = string
}