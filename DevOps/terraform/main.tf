provider "aws" {
region     = "eu-central-1"
access_key = var.access_key
secret_key = var.secret_key
}

data "aws_ami" "ubuntu" {
    most_recent = true
    filter {
        name   = "name"
        values = ["ubuntu/images/hvm-ssd/ubuntu-focal-20.04-amd64-server-*"]
    }
    filter {
        name   = "virtualization-type"
        values = ["hvm"]
    }
    owners = ["099720109477"] # Canonical
  

}

data "aws_vpc" "existing_VPC" {
    filter {
        name   = "tag:Name"
        values = ["Prim-VPC"]
    }
  
}

data "aws_internet_gateway" "existing_IGW" {
    filter {
        name   = "attachment.vpc-id"
        values = [data.aws_vpc.existing_VPC.id]
    }
  
}

resource "aws_subnet" "public_subnet" {
    vpc_id            = data.aws_vpc.existing_VPC.id
    cidr_block        = "10.0.1.0/24"
    availability_zone = "eu-central-1b"
    tags = {
        Name = "Public-Subnet"
    }
  
}

resource "aws_route_table" "public_route_table" {
    vpc_id = data.aws_vpc.existing_VPC.id
    route {
        cidr_block = "0.0.0.0/0"
        gateway_id = data.aws_internet_gateway.existing_IGW.id
    }
    tags = {
        Name = "Public-Route-Table"
    }   

}

resource "aws_route_table_association" "public_route_table_association" {
    subnet_id      = aws_subnet.public_subnet.id
    route_table_id = aws_route_table.public_route_table.id
}

resource "aws_security_group" "security_group" {
    name        = "SSH-Access"
    description = "Allow SSH access"
    vpc_id      = data.aws_vpc.existing_VPC.id

    ingress {
        from_port   = 22
        to_port     = 22
        protocol    = "tcp"
        cidr_blocks = ["0.0.0.0/0"]
    }
    egress {
        from_port   = 0
        to_port     = 0
        protocol    = "-1"
        cidr_blocks = ["0.0.0.0/0"]
    }

    ingress {
        from_port   = 80
        to_port     = 80
        protocol    = "tcp"
        cidr_blocks = ["0.0.0.0/0"]
    }

    ingress {
        from_port   = 443
        to_port     = 443
        protocol    = "tcp"
        cidr_blocks = ["0.0.0.0/0"]
    }

    ingress {
        from_port   = 10250
        to_port     = 10250
        protocol    = "tcp"
        self = true
    }

    # 🔁 Flannel (internal only)
    ingress {
        description = "flannel vxlan"
        from_port   = 8472
        to_port     = 8472
        protocol    = "udp"
        self        = true
    }

    ingress {
        from_port = 30000
        to_port = 32767
        protocol = "tcp"
        cidr_blocks = ["0.0.0.0/0" ]
        

    }

    tags = {
       Name = "worker-SG"
       project_name = "server_Project"

    }

}

data "aws_key_pair" "server_key" {
  key_name = "server-key"
}

resource "aws_eip" "eip" {
    domain = "vpc"
    instance = aws_instance.node.id
    tags = {
        Name = "Worker-Node-EIP"
        project_name = "server_Project"
    }
    depends_on = [ data.aws_internet_gateway.existing_IGW ]
}


resource "aws_instance" "node" {
    ami           = data.aws_ami.ubuntu.id
    instance_type = "c7i-flex.large"
    subnet_id     = aws_subnet.public_subnet.id
    vpc_security_group_ids = [aws_security_group.security_group.id]
    key_name = data.aws_key_pair.server_key.key_name

    tags = {
        Name = "Worker-Node"
        project_name = "server_Project"
    }

    root_block_device {
      volume_size = 256
        volume_type = "gp3"
        delete_on_termination = true
    }
}


output "public_ip" {
  description = "Public IP address of the node "
  value = aws_eip.eip.public_ip
}
output "Instance_id" {
  description = "EC2 Instance ID"
  value = aws_instance.node.id
}

output "vm_admin_username" {
  description = "vm Admin Username"
  value = "ubuntu"
}











