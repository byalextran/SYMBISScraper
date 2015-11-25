require "mechanize"
require "pony"

agent = Mechanize.new
agent.user_agent_alias = 'Mac FireFox'

loop do
  page = agent.get "http://www.youneedabudget.com/download"
  link = page.link_with(:href => /invitationCode/i)
  # link = page.link_with(:href => /liveCaptive/i)

  unless link.nil?
    Pony.mail(:to => "EMAIL", :subject => "invitation link found", :body => link.href)
    puts link.href
    break
  end

  sleep 60
end